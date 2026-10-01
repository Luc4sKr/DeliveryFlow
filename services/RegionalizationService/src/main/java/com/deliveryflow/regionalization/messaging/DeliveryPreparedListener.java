package com.deliveryflow.regionalization.messaging;

import com.deliveryflow.regionalization.model.DeliveryPreparedEvent;
import com.deliveryflow.regionalization.model.PartialDeliveryEvent;
import com.deliveryflow.regionalization.model.RegionAssignedEvent;
import com.deliveryflow.regionalization.service.RegionAssignmentService;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.stereotype.Component;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicReference;

@Component
public class DeliveryPreparedListener {

    private final RegionAssignmentService regionAssignmentService;
    private final Map<String, AggregationState> aggregations = new ConcurrentHashMap<>();

    public DeliveryPreparedListener(RegionAssignmentService regionAssignmentService) {
        this.regionAssignmentService = regionAssignmentService;
    }

    @RabbitListener(queues = "${deliveryflow.rabbitmq.queues.delivery-prepared}")
    @SendTo("${deliveryflow.rabbitmq.exchange}/${deliveryflow.rabbitmq.routing-keys.region-assigned}")
    public RegionAssignedEvent onDeliveryPrepared(PartialDeliveryEvent event) {
        if (event.id() == null) {
            return null;
        }

        var completed = new AtomicReference<AggregationState>();

        aggregations.compute(event.id(), (_, state) -> {
            state = state == null ? new AggregationState() : state;

            state.accept(event);

            if (state.complete()) {
                completed.set(state);
                return null;
            }

            return state;
        });

        var state = completed.get();

        if (state == null) {
            return null;
        }

        return regionAssignmentService.assignRegion(
                new DeliveryPreparedEvent(
                        event.id(),
                        "",
                        state.latitude,
                        state.longitude,
                        state.priority
                )
        );
    }
}
