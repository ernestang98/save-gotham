public class Metal : InteractableObject
{
    private int pointsToAdd = 10;
    private int pointsToDeduct = 10;

    private const string metalText = "Metal";

    public override void AddPoints(string tag)
    {
        if (tag == metalText)
            pointsController.addPoints(pointsToAdd);
        else
            pointsController.addPoints(-1 * pointsToDeduct);
        Destroy(this.gameObject);
    }
}
