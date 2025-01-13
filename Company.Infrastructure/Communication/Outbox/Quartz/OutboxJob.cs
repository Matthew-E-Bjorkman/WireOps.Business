using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using WireOps.Company.Infrastructure.Communication.Outbox.Common;

namespace WireOps.Company.Infrastructure.Communication.Outbox.Quartz;

// TODO: deleting processed messages in separate Job
[DisallowConcurrentExecution]
public class OutboxJob<TProcessor>(TProcessor processor, ILogger<OutboxJob<TProcessor>> logger, int partition = 1)
    : IJob
    where TProcessor : class, TransactionalOutboxProcessor
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            logger.LogDebug("Outbox processing started. {OutboxProcessorType}, {Partition}",
                processor.GetType().Name, partition);
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var processingResult = await processor.ProcessSingleBatch(partition, context.CancellationToken);
                switch (processingResult)
                {
                    case BatchProcessingResult.FullBatchProcessed:
                        break;
                    case BatchProcessingResult.NotFullBatchProcessed:
                    case BatchProcessingResult.TemporaryError:
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(processingResult));
                }
            }
            logger.LogDebug("Outbox processing ended. {OutboxProcessorType}, {Partition}",
                processor.GetType().Name, partition);
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            logger.LogCritical(e,
                "Unexpected exception in outbox processor: {OutboxProcessorType}",
                processor.GetType().Name);
        }
    }
}