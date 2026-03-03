# =========================
#  BUILD STAGE
# =========================
FROM node:20-alpine AS build
WORKDIR /app

# copy package files and install deps
COPY src/ensek-meter-readings-ui/package*.json ./
RUN npm install

# copy the rest of the app
COPY  src/ensek-meter-readings-ui/. .

# build Angular app
RUN npm run build

# =========================
#  RUNTIME STAGE (NGINX)
# =========================
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# remove default nginx static assets
RUN rm -rf ./*

# copy built Angular files
COPY --from=build /app/dist/ensek-meter-readings-ui ./

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
