package com.deliveryflow.routing.config;

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
    @Value("${deliveryflow.rabbitmq.queues.routing}")
    private String routingQueueName;
    @Value("${deliveryflow.rabbitmq.routing-keys.region-assigned}")
    private String regionAssignedRoutingKey;

    @Bean
    public TopicExchange exchange() {
        return new TopicExchange(exchange);
    }

    @Bean
    public Queue routingQueue() {
        return new Queue(routingQueueName, true);
    }

    @Bean
    public Binding routingBinding(Queue routingQueue, TopicExchange exchange) {
        return BindingBuilder.bind(routingQueue).to(exchange).with(regionAssignedRoutingKey);
    }

    @Bean
    public MessageConverter jsonMessageConverter() {
        return new JacksonJsonMessageConverter();
    }
}
