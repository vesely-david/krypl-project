FROM node:8.0.0

# Override the base log level (info).
ENV NPM_CONFIG_LOGLEVEL warn

# Install and configure `serve`.
RUN npm install -g serve
CMD serve -p 80 dist

# Install all dependencies of the current project.
COPY package.json package.json
COPY package-lock.json package-lock.json
RUN npm install

# Copy all local files into the image.
COPY . .

# Build for production.
# CMD npm start -s
RUN npm run build --production