FROM eclipse-temurin:25-jdk AS build
WORKDIR /app

# Install Maven
RUN apt-get update && apt-get install -y maven

COPY pom.xml .

# Download dependencies (this will be cached if pom.xml doesn't change)
RUN mvn dependency:go-offline -B || true

COPY src src
RUN mvn clean package -DskipTests

# Build runtime image
FROM eclipse-temurin:25-jre
WORKDIR /app
COPY --from=build /app/target/regionalization-service-*.jar app.jar

EXPOSE 8080
ENTRYPOINT ["java", "-jar", "app.jar"]