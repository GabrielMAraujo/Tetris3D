using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Moq;
using UnityEngine.UI;

namespace Tests
{
    public class ScoreTest
    {
        //Script reference
        private Score score;

        //Class mocks
        private Mock<Text> scoreTextMock = new Mock<Text>();
        private Mock<Game> gameMock = new Mock<Game>();
        private Mock<BlockController> blockControllerMock = new Mock<BlockController>();


        private GameObject CreateScoreGameObject(GameObject prefab)
        {
            GameObject scoreGO = MonoBehaviour.Instantiate(prefab);
            score = scoreGO.GetComponent<Score>();
            score.game = gameMock.Object;
            score.blockController = blockControllerMock.Object;
            score.currentScoreText = scoreTextMock.Object;
            score.highScoreText = scoreTextMock.Object;

            return scoreGO;
        }

        //Tests if score goes up when block is added
        [Test]
        public void AddBlockScoreWithoutSpeedTest()
        {
            GameObject scorePrefab = Resources.Load<GameObject>("Prefabs/Score");
            GameObject scoreGO = CreateScoreGameObject(scorePrefab);
            score = scoreGO.GetComponent<Score>();

            int initialScore = score.GetCurrentScore();

            score.AddBlockScore(false);

            int expectedScore = initialScore + score.scoreData.pointsWithoutSpeed;

            Assert.AreEqual(score.GetCurrentScore(), expectedScore);

            Object.Destroy(score.gameObject);
        }

        [Test]
        public void AddBlockScoreWithSpeedTest()
        {
            GameObject scorePrefab = Resources.Load<GameObject>("Prefabs/Score");
            GameObject scoreGO = CreateScoreGameObject(scorePrefab);
            score = scoreGO.GetComponent<Score>();

            int initialScore = score.GetCurrentScore();

            score.AddBlockScore(true);

            int expectedScore = initialScore + score.scoreData.pointsWithSpeed;

            Assert.AreEqual(score.GetCurrentScore(), expectedScore);

            Object.Destroy(score.gameObject);
        }

        [Test]
        public void AddRowScoreTest()
        {
            GameObject scorePrefab = Resources.Load<GameObject>("Prefabs/Score");
            GameObject scoreGO = CreateScoreGameObject(scorePrefab);
            score = scoreGO.GetComponent<Score>();

            int initialScore = score.GetCurrentScore();

            score.AddRowScore();

            int expectedScore = initialScore + score.scoreData.pointsPerRow;

            Assert.AreEqual(score.GetCurrentScore(), expectedScore);

            Object.Destroy(score.gameObject);
        }

    }
}
