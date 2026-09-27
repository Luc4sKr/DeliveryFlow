FROM docker.io/library/golang:1.25 AS build

WORKDIR /src

COPY . .

RUN make build


FROM gcr.io/distroless/static-debian12 as runtime

WORKDIR /app

COPY --from=build /src/bin .

ENTRYPOINT ["./geocoder"]
