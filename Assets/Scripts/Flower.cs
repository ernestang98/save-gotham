public class Flower : InteractableObject
{
    private int pointsToAdd = 20;
    private int pointsToDeduct = 20;

    private const string flowerText = "Flower";

    public override void AddPoints(string tag)
    {
        if (tag == flowerText)
            pointsController.addPoints(pointsToAdd);
        else
        {
            pointsController.addPoints(-1 * pointsToDeduct);
            Destroy(this.gameObject);
        }
    }
}
