package com.deliveryflow.warehouse.config;
import org.springframework.amqp.core.*;
import org.springframework.amqp.support.converter.JacksonJsonMessageConverter;
import org.springframework.amqp.support.converter.MessageConverter;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {
    @Value("${deliveryflow.rabbitmq.exchange}")
    private String exchange;
    @Value("${deliveryflow.rabbitmq.queues.warehouse}")
    private String warehouseQueueName;
    @Value("${deliveryflow.rabbitmq.routing-keys.region-assigned}")
    private String regionAssignedRoutingKey;

    @Bean public TopicExchange exchange() { return new TopicExchange(exchange); }
    @Bean public Queue warehouseQueue() { return new Queue(warehouseQueueName, true); }
    @Bean public Binding warehouseBinding(Queue warehouseQueue, TopicExchange exchange) {
        return BindingBuilder.bind(warehouseQueue).to(exchange).with(regionAssignedRoutingKey);
    }
    @Bean public MessageConverter jsonMessageConverter() { return new JacksonJsonMessageConverter(); }
}
