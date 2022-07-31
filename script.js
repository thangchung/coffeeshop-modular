// cat script.js | docker run --rm -i grafana/k6 run -

import http from 'k6/http';
import { sleep } from 'k6';
export const options = {
  vus: 10,
  duration: '30s',
};
export default function () {
  http.get('http://host.docker.internal:5000/v1/api/fulfillment-orders');
  sleep(1);
}