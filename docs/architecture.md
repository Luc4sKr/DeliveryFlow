# 1. Descrição do Cenário

## Contextualização

O sistema é voltado para o gerenciamento e processamento de entregas em uma empresa de logística. Ao receber uma nova entrega, diferentes serviços distribuídos são responsáveis por validar os dados, realizar a geocodificação do endereço, definir a prioridade, determinar a região, calcular a rota e identificar o armazém responsável.

## Justificativa da Comunicação Assíncrona

A comunicação assíncrona via RabbitMQ permite que os serviços processem as etapas de forma independente, sem que um serviço precise aguardar diretamente a resposta de outro. Isso reduz o acoplamento entre os componentes e permite que diferentes etapas sejam processadas em paralelo, além de facilitar a escalabilidade e a tolerância a falhas.

# 2. Arquitetura da Solução

## Componentes do Sistema

- **Delivery Service:** recebe uma nova solicitação de entrega e inicia o processamento.

- **Validation Service:** valida os dados da entrega, verificando se as informações necessárias estão corretas.

- **Geocoding Service:** converte o endereço da entrega em coordenadas geográficas.

- **Priority Service:** determina a prioridade da entrega de acordo com suas características.

- **Regionalization Service:** determina a região responsável pela entrega com base nos dados processados.

- **Routing Service:** calcula a rota necessária para realizar a entrega.

- **Warehouse Service:** identifica o armazém responsável pela entrega.

- **RabbitMQ:** responsável pela comunicação assíncrona entre os serviços, armazenando e distribuindo as mensagens entre produtores e consumidores.

## Fluxo de Mensagens

O fluxo inicia no **Delivery Service**, que publica uma mensagem `DeliveryCreated` no RabbitMQ. Essa mensagem é consumida simultaneamente pelos serviços de **Validation**, **Geocoding** e **Priority**.

Após o processamento, cada serviço publica uma mensagem de conclusão (`ValidationCompleted`, `GeocodingCompleted` e `PriorityCompleted`). Essas mensagens são utilizadas pelo **Regionalization Service**, que determina a região da entrega e publica uma mensagem `RegionAssigned`.

Por fim, a mensagem é consumida pelo **Routing Service**, responsável pelo cálculo da rota, e pelo **Warehouse Service**, responsável pela identificação do armazém.

## Estratégias de Escalabilidade, Confiabilidade e Tolerância a Falhas

- **Escalabilidade:** os serviços podem ser executados em múltiplas instâncias, permitindo distribuir o processamento das mensagens entre diferentes consumidores.

- **Confiabilidade:** as mensagens podem ser mantidas nas filas até que sejam processadas e confirmadas pelos consumidores, reduzindo o risco de perda de mensagens.

- **Tolerância a falhas:** caso um consumidor fique indisponível, as mensagens podem permanecer na fila para serem processadas posteriormente por outra instância disponível. O uso de mecanismos como **acknowledgement** e **requeue** permite recuperar mensagens que não foram processadas corretamente.