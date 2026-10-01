package com.deliveryflow.warehouse.model;
public record RegionAssignedEvent(String deliveryId, String assignedRegion, double latitude, double longitude) {}
