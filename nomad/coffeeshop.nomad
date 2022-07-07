variable "domain" {
  type        = string
  default     = "coffeeshop.local"
  description = "hostname"
}

job "coffeeshop" {
  datacenters = ["dc1"]
  type        = "service"

  group "coffeeshop-frontend" {
    count = 1

    network {
      port "http" {
        static = 5000
      }
    }

    service {
      name = "coffeeshop"
      port = "http"

      tags = [
        "traefik.enable=true",
        "traefik.http.routers.coffeeshop.rule=Host(`${var.domain}`)",
      ]

      check {
        port     = "http"
        name     = "alive"
        type     = "tcp"
        interval = "10s"
        timeout  = "2s"
      }
    }

    task "server" {
      driver = "raw_exec"

      artifact {
        source = "git::https://github.com/thangchung/coffeeshop-modular"
        destination = "local/repo"
      }

      env {
        ConnectionStrings__coffeeshopdb = "Server=${NOMAD_IP_http};Port=5432;Database=postgres;User Id=postgres;Password=P@ssw0rd"
      }

      config {
        command = "bash"
        args = [
          "-c",
          "cd local/repo/ && dotnet build && dotnet run --project src/coffeeshop/CoffeeShop.csproj"
        ]
      }
    }
  }

  group "coffeeshop-backend" {
    network {
      port "db" {
        static = 5432
      }
    }

    task "postgres" {
      driver = "docker"

      service {
        name = "postgresql"
        port = "db"

        check {
          name     = "alive"
          type     = "tcp"
          interval = "10s"
          timeout  = "2s"
        }
      }

      env {
        POSTGRES_USER = "postgres"
        POSTGRES_PASSWORD = "P@ssw0rd"
        POSTGRES_DB = "postgres"
      }

      config {
        image = "postgres:14-alpine"
        ports = ["db"]
      }
    }
  }
}