FROM node:22-alpine3.22 AS builder

ARG VITE_APP_HOST
ENV VITE_APP_HOST=${VITE_APP_HOST}

RUN apk add --no-cache git

WORKDIR /usr/src/app

COPY package*.json ./

RUN npm ci

COPY . . 

RUN npm run build

FROM nginx:1.24-alpine

COPY --from=builder /usr/src/app/dist ./usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]