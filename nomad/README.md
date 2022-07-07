# Get starting with nomad

```bash
> nomad job run coffeeshop.nomad
```

You should see logs as

```
==> 2022-07-07T22:41:45+07:00: Monitoring evaluation "e5d1a1ac"
    2022-07-07T22:41:45+07:00: Evaluation triggered by job "coffeeshop"
    2022-07-07T22:41:45+07:00: Allocation "67dcd659" created: node "2a3fee52", group "coffeeshop-backend"
    2022-07-07T22:41:45+07:00: Allocation "806755c8" created: node "2a3fee52", group "coffeeshop-frontend"
==> 2022-07-07T22:41:46+07:00: Monitoring evaluation "e5d1a1ac"
    2022-07-07T22:41:46+07:00: Evaluation within deployment: "08562339"
    2022-07-07T22:41:46+07:00: Allocation "67dcd659" status changed: "pending" -> "running" (Tasks are running)
    2022-07-07T22:41:46+07:00: Evaluation status changed: "pending" -> "complete"
==> 2022-07-07T22:41:46+07:00: Evaluation "e5d1a1ac" finished with status "complete"
==> 2022-07-07T22:41:46+07:00: Monitoring deployment "08562339"
  ⠇ Deployment "08562339" in progress...

    2022-07-07T22:44:30+07:00
    ID          = 08562339
    Job ID      = coffeeshop
    Job Version = 0
    Status      = running
    Description = Deployment is running

    Deployed
    Task Group           Desired  Placed  Healthy  Unhealthy  Progress Deadline
    coffeeshop-backend   1        1       0        0          2022-07-07T22:51:45+07:00
    coffeeshop-frontend  1        1       0        0          2022-07-07T22:51:45+07:00
```

Then waiting until you see `coffeeshop-backend` and `coffeeshop-frontend` become `healthy`

Check status of workload and networking at [`http://localhost:4646/`](http://localhost:4646/)

Finally, go to [`http://localhost:5000`](http://localhost:5000) to test it

Notes:
- `Nomad` run inside WSL2 on Windows 11 => you can use Linux container (Docker for Desktop on Windows 11)
- Make sure no process occupied port `4646` on host (Windows 11)
  - List out by using `netstat -ano | findstr :4646`
  - Kill process by it id by using `taskkill /PID <id> /F`
- nomad version: `Nomad v1.2.8 (885a03085ecc7357de4cb0b8ad1804f0532fc756)` => version 1.3.1 have got an error, I don't know why
- Start nomad in WSL2: `sudo nomad agent -dev -bind 0.0.0.0 -log-level INFO`