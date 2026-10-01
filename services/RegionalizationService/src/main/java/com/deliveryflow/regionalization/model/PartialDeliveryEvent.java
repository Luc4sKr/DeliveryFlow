package com.deliveryflow.regionalization.model;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonSubTypes;
import com.fasterxml.jackson.annotation.JsonTypeInfo;
import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.PropertyNamingStrategies.UpperCamelCaseStrategy;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

@JsonTypeInfo(
        use = JsonTypeInfo.Id.NAME,
        include = JsonTypeInfo.As.PROPERTY,
        property = "Service"
)
@JsonSubTypes({
        @JsonSubTypes.Type(value = PartialDeliveryEvent.GeocodingEvent.class, name = "GeocodingService"),
        @JsonSubTypes.Type(value = PartialDeliveryEvent.PriorityEvent.class, name = "PriorityService"),
        @JsonSubTypes.Type(value = PartialDeliveryEvent.ValidationEvent.class, name = "ValidationService")
})
@JsonIgnoreProperties(ignoreUnknown = true)
public sealed interface PartialDeliveryEvent {

    String id();

    @JsonNaming(UpperCamelCaseStrategy.class)
    record GeocodingEvent(String id, Location location) implements PartialDeliveryEvent {
    }

    @JsonNaming(UpperCamelCaseStrategy.class)
    record Location(Double latitude, Double longitude) {
    }

    @JsonNaming(UpperCamelCaseStrategy.class)
    record PriorityEvent(String id, String priority) implements PartialDeliveryEvent {
    }

    @JsonNaming(UpperCamelCaseStrategy.class)
    record ValidationEvent(String id, String status) implements PartialDeliveryEvent {
    }
}
