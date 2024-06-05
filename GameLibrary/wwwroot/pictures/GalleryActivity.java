package me.ndres.tflitedemo;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Rect;
import android.graphics.drawable.BitmapDrawable;
import android.net.Uri;
import android.os.Environment;
import android.provider.MediaStore;
import android.os.Bundle;
import android.text.SpannableString;
import android.text.SpannableStringBuilder;
import android.util.Log;
import android.util.SparseArray;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.gms.vision.Frame;
import com.google.android.gms.vision.face.Face;
import com.google.android.gms.vision.face.FaceDetector;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;

public class GalleryActivity extends Activity {

    private static final int PICK_IMAGE = 100;
    private static final String TAG = "TfLiteDemo";
    private Classifier classifier;
    private SparseArray<Face> faces = null;
    private FaceDetector faceDetector;
    ImageView imageView;
    private TextView textView;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gallery);

        Intent gallery = new Intent(Intent.ACTION_PICK, MediaStore.Images.Media.INTERNAL_CONTENT_URI);
        startActivityForResult(gallery, PICK_IMAGE);

        try {
            classifier = new ImageClassifier(this);
        } catch (IOException e) {
            e.printStackTrace();
        }

    }


    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data){
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK && requestCode == PICK_IMAGE){
            Uri imageUri = data.getData();
            imageView = findViewById(R.id.imageViewGallery);
            imageView.setImageURI(imageUri);
            textView = findViewById(R.id.text);

            faceDetector = new FaceDetector.Builder(this)
                    .setMode(FaceDetector.FAST_MODE)
                    .setTrackingEnabled(false)
                    .setLandmarkType(FaceDetector.NO_LANDMARKS)
                    .build();

            faceDetector.isOperational();
            Bitmap bitmap = ((BitmapDrawable)imageView.getDrawable()).getBitmap();
            SpannableStringBuilder textToShow = new SpannableStringBuilder();
/*
            bitmap = Bitmap.createScaledBitmap(bitmap, 48, 48, true);


            classifier.classifyFrame(bitmap, textToShow);
            */

            Frame frame = new Frame.Builder().setBitmap(bitmap).build();
            faces = faceDetector.detect(frame);

            for (int i = 0; i < faces.size(); ++i) {
                Face face = faces.valueAt(i);

                Rect rect = new Rect(Math.round(face.getPosition().x), Math.round(face.getPosition().y),
                        Math.round(face.getWidth() + face.getPosition().x), Math.round(face.getPosition().y + face.getHeight()));

                //  Be sure that there is at least 1px to slice.
                assert (rect.left < rect.right && rect.top < rect.bottom);
                //  Create our resulting image (150--50),(75--25) = 200x100px
                Bitmap resultBmp = Bitmap.createBitmap(rect.right - rect.left, rect.bottom - rect.top, Bitmap.Config.ARGB_8888);
                //  draw source bitmap into resulting image at given position:
                new Canvas(resultBmp).drawBitmap(bitmap, -rect.left, -rect.top, null);

                Bitmap resized = Bitmap.createScaledBitmap(resultBmp, 48, 48, true);

                // enforce the probabilities artificially, since this is a static image, so the classifier will only get one frame
                for (int iter = 1; iter <= 9; iter++) {
                    classifier.classifyFrame(resized, new SpannableStringBuilder());
                }

                // get the actual text to show
                classifier.classifyFrame(resized, textToShow);
            }

            showToast(textToShow);
        }
    }

    private void showToast(SpannableStringBuilder builder) {
        final Activity activity = this;
        if (activity != null) {
            activity.runOnUiThread(
                    new Runnable() {
                        @Override
                        public void run() {
                            textView.setText(builder, TextView.BufferType.SPANNABLE);
                        }
                    });
        }
    }
}
