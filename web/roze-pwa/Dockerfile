FROM node:20 AS build
WORKDIR /app
COPY package.json package-lock.json* .npmrc* ./
RUN npm install --force || npm install
COPY . .
RUN npm run build

FROM node:20 AS serve
WORKDIR /app
COPY --from=build /app/dist ./dist
RUN npm install -g serve
EXPOSE 4173
CMD ["serve", "-s", "dist", "-l", "4173"]
