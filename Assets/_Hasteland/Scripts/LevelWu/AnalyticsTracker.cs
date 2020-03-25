using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AnalyticsTracker<T>
{
    void TrackAnalytics(T objectTracked);
}
