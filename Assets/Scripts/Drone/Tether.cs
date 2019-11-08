using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    LineRenderer lineRenderer;
    List<RopeSegment> ropeSegments = new List<RopeSegment>();
    [SerializeField] float ropeSegLen = 0.25f;
    [SerializeField] int numSegments = 20;
    [SerializeField] float lineWidth = 0.1f;
    [SerializeField] Vector2 forceGravity = new Vector2(0.0f, -1.0f);
    [SerializeField] Vector2 offset = new Vector2(0.0f, 2.0f);

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = transform.position;

        for (int i = 0; i < numSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
        Simulate();
    }

    void DrawRope()
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[numSegments];
        for (int i = 0; i < numSegments; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    void Simulate()
    {
        for (int i = 0; i < this.numSegments; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 vel = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += (vel + forceGravity * Time.deltaTime);
            ropeSegments[i] = firstSegment;
        }

        for (int i = 0; i < 50; i++)
        {
            ApplyConstraints();
        }
    }

    void ApplyConstraints()
    {
        RopeSegment firstSegment = ropeSegments[0];
        Vector2 startRopePosition = new Vector2(transform.position.x, transform.position.y) + offset;
        firstSegment.posNow = startRopePosition;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < numSegments - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist-ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if(dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmt = changeDir * error;

            if (i != 0)
            {
                firstSeg.posNow -= changeAmt * error;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmt * 0.5f;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmt;
                ropeSegments[i + 1] = secondSeg;
            }
        }

    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
