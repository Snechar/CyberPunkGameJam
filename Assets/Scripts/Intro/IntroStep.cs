using UnityEngine.UI;

public abstract class IntroStep {
    abstract public void Act();
    abstract public bool Completed();

    public static IntroStep FadeIn(Image img, float duration) {
        return new FadeImage(true, img, duration);
    }

    public static IntroStep FadeOut(Image img, float duration) {
        return new FadeImage(false, img, duration);
    }
}

public class FadeImage : IntroStep {
    public Image img { get; private set; }
    public FadeImage(bool fadeIn, Image img, float duration) {
        this.img = img;
    }

    public override void Act() {
    }

    public override bool Completed() {
        return true;
    }
}