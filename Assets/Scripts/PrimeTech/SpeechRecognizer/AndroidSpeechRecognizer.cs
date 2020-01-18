using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidSpeechRecognizer {

    /**
     * Key used to retrieve an {@code ArrayList<String>} from the {@link Bundle} passed to the
     * {@link RecognitionListener#onResults(Bundle)} and
     * {@link RecognitionListener#onPartialResults(Bundle)} methods. These strings are the possible
     * recognition results, where the first element is the most likely candidate.
     */
    public static readonly string RESULTS_RECOGNITION = "results_recognition";

    /**
     * Key used to retrieve a float array from the {@link Bundle} passed to the
     * {@link RecognitionListener#onResults(Bundle)} and
     * {@link RecognitionListener#onPartialResults(Bundle)} methods. The array should be
     * the same size as the ArrayList provided in {@link #RESULTS_RECOGNITION}, and should contain
     * values ranging from 0.0 to 1.0, or -1 to represent an unavailable confidence score.
     * <p>
     * Confidence values close to 1.0 indicate high confidence (the speech recognizer is confident
     * that the recognition result is correct), while values close to 0.0 indicate low confidence.
     * <p>
     * This value is optional and might not be provided.
     */
    public static readonly string CONFIDENCE_SCORES = "confidence_scores";

    /** Network operation timed out. */
    public static readonly int ERROR_NETWORK_TIMEOUT = 1;

    /** Other network related errors. */
    public static readonly int ERROR_NETWORK = 2;

    /** Audio recording error. */
    public static readonly int ERROR_AUDIO = 3;

    /** Server sends error status. */
    public static readonly int ERROR_SERVER = 4;

    /** Other client side errors. */
    public static readonly int ERROR_CLIENT = 5;

    /** No speech input */
    public static readonly int ERROR_SPEECH_TIMEOUT = 6;

    /** No recognition result matched. */
    public static readonly int ERROR_NO_MATCH = 7;

    /** RecognitionService busy. */
    public static readonly int ERROR_RECOGNIZER_BUSY = 8;

    /** Insufficient permissions */
    public static readonly int ERROR_INSUFFICIENT_PERMISSIONS = 9;
}