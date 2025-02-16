public class CoinPool : Pool<Coin>
{
    protected override Coin CreateNewObject()
    {
        Coin newCoin = Instantiate(Template, Container);

        newCoin.Initialize(Container);
        newCoin.Deactivate();
        ObjectsPool.Add(newCoin);

        return newCoin;
    }
}
