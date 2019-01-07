using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using weka.classifiers.bayes;
using weka.classifiers.evaluation;
using weka.core;

public class Classification : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Instances insts = new Instances(new java.io.FileReader("C:\\Users\\leonb\\Desktop\\imdb.arff"));
        insts.setClassIndex(insts.numAttributes() - 1);

        Evaluation eval = null;

       
        NaiveBayes nB = new NaiveBayes();
       
        nB.buildClassifier(insts);
        eval = new Evaluation(insts);
        eval.crossValidateModel(nB, insts, 10, new java.util.Random(1));
        UnityEngine.Debug.Log(eval.toSummaryString());
        if (eval.correct() > 80)
        {
            SceneManager.LoadScene("GameOver");

        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
