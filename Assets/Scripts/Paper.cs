public class Paper : InteractableObject
{
    private int pointsToAdd = 10;
    private int pointsToDeduct = 10;

    private const string paperText = "Paper";

    public override void AddPoints(string tag)
    {
        if (tag == paperText)
            pointsController.addPoints(pointsToAdd);
        else
            pointsController.addPoints(-1 * pointsToDeduct);
        Destroy(this.gameObject);
    }
}
