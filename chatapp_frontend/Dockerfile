# Stage 1: Build the React application
FROM node:20-alpine AS build
WORKDIR /app

# Accept build arguments
ARG REACT_APP_API_URL
ARG REACT_APP_SIGNALR_URL_CHAT
ARG REACT_APP_SIGNALR_URL_CALL

# Set as environment variables for build
ENV REACT_APP_API_URL=$REACT_APP_API_URL
ENV REACT_APP_SIGNALR_URL_CHAT=$REACT_APP_SIGNALR_URL_CHAT
ENV REACT_APP_SIGNALR_URL_CALL=$REACT_APP_SIGNALR_URL_CALL

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install --legacy-peer-deps

# Copy source and build
COPY . .
RUN npm run build

# Stage 2: Serve the application using Nginx
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
