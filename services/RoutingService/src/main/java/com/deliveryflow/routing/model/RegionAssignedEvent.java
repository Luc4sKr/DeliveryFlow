package com.deliveryflow.routing.model;
public record RegionAssignedEvent(String deliveryId, String assignedRegion, double latitude, double longitude) {}
