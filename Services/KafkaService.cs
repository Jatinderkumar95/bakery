using Confluent.Kafka;

namespace bakery.Services
{
    public class KafkaService
    {
        public void DefineQueue()
        {
            var is_data_structure = true;
            var follow_FIFO = true;
            var helps_to_store_manage_and_process_message_in_same_order_from_publisher_to_multiple_consumers = true;
            var decouple_systems = true;
            var fault_tolerance = "If a consumer fails while processing a message, the message can be re-queued and processed by another consumer. This ensures that no messages are lost and all tasks are eventually completed.";
            var user_cases = "Order Processing: An e-commerce platform uses a queue to handle order processing. Orders are placed into the queue by the web application, and various services (inventory, payment, shipping) consume and process these orders.\r\nLog Aggregation: A logging system collects logs from different services and places them into a queue. A log processing service then consumes these logs for analysis and storage.\r\nTask Scheduling: A task scheduler places tasks into a queue, and worker nodes consume and execute these tasks based on availability.";

        }
        public async Task RetryQueueAtProducer()
        {
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092"
            };
            using var producerBuilder = new ProducerBuilder<Null,string>(producerConfig).Build();
            await producerBuilder.ProduceAsync("retry-queue", new Message<Null, string> { Value = "abc" });
        }
        public async Task RetryQueueAtConsumer()
        {
            var producerConfig = new ConsumerConfig()
            {
                BootstrapServers = "localhost:9092",
                GroupId = "group-1",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using var producerBuilder = new ProducerBuilder<Null, string>(producerConfig).Build();
            await producerBuilder.ProduceAsync("retry-queue", new Message<Null, string> { Value = "abc" });
        }
        public void DelayQueue()
        {

        }
        public void DeadLetterQueue()
        {

        }
    }
}
