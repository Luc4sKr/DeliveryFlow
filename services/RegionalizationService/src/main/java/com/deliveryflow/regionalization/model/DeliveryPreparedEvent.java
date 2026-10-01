package com.deliveryflow.regionalization.model;

public record DeliveryPreparedEvent(
    String deliveryId,
    String address,
    double latitude,
    double longitude,
    String priorityLevel
) {}
