using System;
using System.Collections.Generic;
using System.Data;

namespace WhatScoreCanIGetByGuessing
{
    public class ListGenerator
    {
        public static List<int> generateEqualList()
        {
            List<int> retVal = new List<int>(100);
            int[] ansCnt = new int[] { 25, 25, 25, 25, 25 };
            for (int i = 0; i < 100; i++)
            {
                while (true)
                {
                    int answer = new Random().Next(0, 4);
                    if (ansCnt[answer] != 0)
                    {
                        retVal.Add(answer);
                        ansCnt[answer]--;
                        break;
                    }
                }
            }
            return retVal;
        }

        public static List<int> generatePureRandomList()
        {
            List<int> retVal = new List<int>(100);
            for (int i = 0; i < 100; i++)
            {
                retVal.Add(new Random().Next(0, 4));
            }
            return retVal;
        }

        public static List<int> generateOneNumberList()
        {
            int targetNumber = new Random().Next(0, 4);
            List<int> retVal = new List<int>(100);
            for (int i = 0; i < 100; i++)
            {
                retVal.Add(targetNumber);
            }
            return retVal;
        }
    }
    class TestSheet
    {
        private bool isPureRandom;
        private List<int> answerList;

        public TestSheet() { }
        public TestSheet(bool isPureRandom)
        {
            this.isPureRandom = isPureRandom;
            answerList = new List<int>(100);
        }
        public List<int> getSheet()
        {
            return this.answerList;
        }

        public void generateTestSheet()
        {
            if (isPureRandom)
            {
                answerList = ListGenerator.generatePureRandomList();
            }
            else
            {
                answerList = ListGenerator.generateEqualList();
            }
        }
    }

    class TestTaker
    {
        private TestTakerType type;
        private List<bool> markResult;
        private List<int> guessResult = new List<int>(100);

        public TestTaker() { }
        public TestTaker(TestTakerType type)
        {
            this.type = type;
            markResult = new List<bool>(100);
            guessResult = new List<int>(100);
        }
        public int doMarking(List<int> answer)
        {
            for(int i=0; i<100; i++)
            {
                markResult.Add(guessResult[i] == answer[i] ? true : false);
            }
            return markResult.FindAll(element => element == true).Count;
        }
        public void doGuess()
        {
            switch (type)
            {
                case TestTakerType.EqualGuess:
                    guessResult = ListGenerator.generateEqualList();
                    break;

                case TestTakerType.PureRandom:
                    guessResult = ListGenerator.generatePureRandomList();
                    break;

                case TestTakerType.OneNumber:
                    guessResult = ListGenerator.generateOneNumberList();
                    break;

                default:
                    break;
            }
        }
    }

    public enum TestTakerType
    {
        PureRandom,
        EqualGuess,
        OneNumber
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (System.IO.StreamWriter csv = new System.IO.StreamWriter(@"guess.csv"))
            {
                csv.WriteLine("TestSheet #,IsPureRandomTest,Greade_Equal,Grade_PureRandom,Grade_OneNumber");
                // 200개의 시험지에 대해 각각 200명의 답안지를 비교
                for (int i = 0; i < 2; i++)
                {
                    bool isPureRandomTest = true;
                    if (i == 0) { isPureRandomTest = false; }
                    for (int j = 0; j < 100; j++)
                    {
                        TestSheet testSheet = new TestSheet(isPureRandomTest);
                        testSheet.generateTestSheet();
                        for (int k = 0; k < 100; k++)
                        {
                            TestTaker equalTaker = new TestTaker(TestTakerType.EqualGuess);
                            TestTaker pureRandomTaker = new TestTaker(TestTakerType.PureRandom);
                            TestTaker oneNumberTaker = new TestTaker(TestTakerType.OneNumber);

                            equalTaker.doGuess();
                            pureRandomTaker.doGuess();
                            oneNumberTaker.doGuess();

                            int eqRetVal = equalTaker.doMarking(testSheet.getSheet());
                            int prRetVal = pureRandomTaker.doMarking(testSheet.getSheet());
                            int onRetVal = oneNumberTaker.doMarking(testSheet.getSheet());
                            csv.WriteLine("{0},{1},{2},{3},{4}", j, isPureRandomTest, eqRetVal, prRetVal,onRetVal);
                        }
                    }
                }
            }
        }
    }
}
