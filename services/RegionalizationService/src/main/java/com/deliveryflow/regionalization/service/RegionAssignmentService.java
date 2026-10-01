package com.deliveryflow.regionalization.service;

import com.deliveryflow.regionalization.model.DeliveryPreparedEvent;
import com.deliveryflow.regionalization.model.RegionAssignedEvent;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

@Service
public class RegionAssignmentService {

    private static final Logger logger = LoggerFactory.getLogger(RegionAssignmentService.class);

    public RegionAssignedEvent assignRegion(DeliveryPreparedEvent event) {
        logger.info("Assigning region for delivery: {}", event.deliveryId());
        
        String region = determineRegion(event.latitude(), event.longitude());
        
        logger.info("Assigned region {} for delivery: {}", region, event.deliveryId());
        
        return new RegionAssignedEvent(
            event.deliveryId(),
            region,
            event.latitude(),
            event.longitude()
        );
    }

    private String determineRegion(double latitude, double longitude) {
        if (latitude > 40.0) {
            return longitude > -80.0 ? "NORTH_EAST" : "NORTH_WEST";
        }

        return longitude > -80.0 ? "SOUTH_EAST" : "SOUTH_WEST";
    }
}
