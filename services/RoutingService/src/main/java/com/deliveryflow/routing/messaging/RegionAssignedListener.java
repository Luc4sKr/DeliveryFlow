package com.deliveryflow.routing.messaging;
import com.deliveryflow.routing.model.RegionAssignedEvent;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;

@Component
public class RegionAssignedListener {
    private static final Logger logger = LoggerFactory.getLogger(RegionAssignedListener.class);
    @RabbitListener(queues = "${deliveryflow.rabbitmq.queues.routing}")
    public void onRegionAssigned(RegionAssignedEvent event) {
        logger.info("Received RegionAssignedEvent for deliveryId: {}", event.deliveryId());
        logger.info("Mocking route calculation for lat: {}, lon: {}", event.latitude(), event.longitude());
    }
}
