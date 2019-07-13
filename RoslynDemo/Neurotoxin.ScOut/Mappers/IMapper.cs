namespace Neurotoxin.ScOut.Mappers
{
    public interface IMapper<TIn, TOut>
    {
        TOut Map(TIn input);
    }
}