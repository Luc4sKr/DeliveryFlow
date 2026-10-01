package com.deliveryflow.regionalization.messaging;

import com.deliveryflow.regionalization.model.PartialDeliveryEvent;
import com.deliveryflow.regionalization.model.PartialDeliveryEvent.GeocodingEvent;
import com.deliveryflow.regionalization.model.PartialDeliveryEvent.PriorityEvent;
import com.deliveryflow.regionalization.model.PartialDeliveryEvent.ValidationEvent;

class AggregationState {

    boolean geocoded;
    boolean prioritized;
    boolean validated;

    double latitude;
    double longitude;
    String priority = "";

    void accept(PartialDeliveryEvent event) {
        switch (event) {
            case GeocodingEvent e -> {
                geocoded = true;

                if (e.location() != null) {
                    latitude = e.location().latitude();
                    longitude = e.location().longitude();
                }
            }

            case PriorityEvent e -> {
                prioritized = true;

                if (e.priority() != null) {
                    priority = e.priority();
                }
            }

            case ValidationEvent _ ->
                    validated = true;
        }
    }

    boolean complete() {
        return geocoded && prioritized && validated;
    }
}
