package com.deliveryflow.regionalization.model;

public record RegionAssignedEvent(
    String deliveryId,
    String assignedRegion,
    double latitude,
    double longitude
) {}
