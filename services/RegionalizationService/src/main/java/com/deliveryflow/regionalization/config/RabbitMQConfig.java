package com.deliveryflow.regionalization.config;

import org.springframework.amqp.core.*;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.JacksonJsonMessageConverter;
import org.springframework.amqp.support.converter.MessageConverter;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitMQConfig {

    @Value("${deliveryflow.rabbitmq.exchange}")
    private String exchange;

    @Value("${deliveryflow.rabbitmq.queues.delivery-prepared}")
    private String deliveryPreparedQueueName;

    @Value("${deliveryflow.rabbitmq.routing-keys.delivery-prepared}")
    private String deliveryPreparedRoutingKey;

    @Bean
    public TopicExchange exchange() {
        return new TopicExchange(exchange);
    }

    @Bean
    public Queue deliveryPreparedQueue() {
        return new Queue(deliveryPreparedQueueName, true);
    }

    @Bean
    public Binding deliveryPreparedBinding(Queue deliveryPreparedQueue, TopicExchange exchange) {
        return BindingBuilder.bind(deliveryPreparedQueue).to(exchange).with(deliveryPreparedRoutingKey);
    }

    @Bean
    public MessageConverter jsonMessageConverter() {
        return new JacksonJsonMessageConverter();
    }

    @Bean
    public RabbitTemplate rabbitTemplate(ConnectionFactory connectionFactory) {
        RabbitTemplate template = new RabbitTemplate(connectionFactory);
        template.setMessageConverter(jsonMessageConverter());
        return template;
    }
}
