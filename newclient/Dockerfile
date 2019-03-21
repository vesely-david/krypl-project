FROM node:11.10.0 as react-build

# Override the base log level (info).

# Install and configure `serve`.
# RUN npm install -g serve
# CMD serve -p 80 build

# Install all dependencies of the current project.
# COPY package.json package.json
# COPY yarn.lock yarn.lock
# RUN yarn install

# # Copy all local files into the image.
# COPY . .

# # Build for production.
# # CMD npm start -s
# RUN yarn run build --production
# ENTRYPOINT ["/bin/bash"]


WORKDIR /app
COPY . ./
RUN yarn install 
RUN yarn build --production

# Stage 2 - the production environment
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=react-build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]