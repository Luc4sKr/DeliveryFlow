package com.deliveryflow.warehouse.messaging;
import com.deliveryflow.warehouse.model.RegionAssignedEvent;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

@Component
public class RegionAssignedListener {
    private static final Logger logger = LoggerFactory.getLogger(RegionAssignedListener.class);
    @RabbitListener(queues = "${deliveryflow.rabbitmq.queues.warehouse}")
    public void onRegionAssigned(RegionAssignedEvent event) {
        logger.info("Received RegionAssignedEvent for deliveryId: {}", event.deliveryId());
        logger.info("Determining warehouse for region: {}", event.assignedRegion());
    }
}
