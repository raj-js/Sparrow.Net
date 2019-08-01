namespace Sparrow.Uow
{
    public interface ITranscationManager
    {
        ITranscationWapper GetOrCreate(TranscationArgs args);
    }
}
