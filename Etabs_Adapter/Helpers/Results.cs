﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structural.Results;
using BH.oM.Common;
using ETABS2016;

namespace BH.Adapter.ETABS
{
    public static partial class Helper
    {
        #region Node Results

        public static List<IResult> GetNodeAcceleration(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            throw new NotImplementedException("Node Acceleration results is not supported yet!");
        }

        public static List<NodeDisplacement> GetNodeDisplacement(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            List<string> loadcaseIds = new List<string>();
            List<string> nodeIds = new List<string>();
            List<NodeDisplacement> nodeDisplacements = new List<NodeDisplacement>();

            int resultCount = 0;
            string[] loadcaseNames = null;
            string[] objects = null;
            string[] elm = null;
            string[] stepType = null;
            double[] stepNum = null;
            double[] fx = null;
            double[] fy = null;
            double[] fz = null;
            double[] mx = null;
            double[] my = null;
            double[] mz = null;

            if (ids == null)
            {
                int nodes = 0;
                string[] names = null;
                model.PointObj.GetNameList(ref nodes, ref names);
                nodeIds = names.ToList();
            }

            if (cases == null)
            {
                int casesCount = 0;
                string[] names = null;
                model.LoadCases.GetNameList(ref casesCount, ref names);
                loadcaseIds = names.ToList();
                model.RespCombo.GetNameList(ref casesCount, ref names);
                loadcaseIds.AddRange(names);
            }

            model.Results.Setup.DeselectAllCasesAndCombosForOutput();

            for (int loadcase = 0; loadcase < loadcaseIds.Count; loadcase++)
            {
                if (model.Results.Setup.SetCaseSelectedForOutput(loadcaseIds[loadcase]) != 0)
                {
                    model.Results.Setup.SetComboSelectedForOutput(loadcaseIds[loadcase]);
                }
            }

            for (int i = 0; i < nodeIds.Count; i++)
            {
                int ret = model.Results.JointDispl(nodeIds[i].ToString(), eItemTypeElm.ObjectElm, ref resultCount, ref objects, ref elm,
                ref loadcaseNames, ref stepType, ref stepNum, ref fx, ref fy, ref fz, ref mx, ref my, ref mz);
                if (ret == 0)
                {
                    for (int j = 0; j < resultCount; j++)
                    {
                        //string step = stepType[j] != null ? stepType[j] == "Max" ? " Max" : stepType[j] == "Min" ? " Min" : "1" : "0";
                        //nodeForces.Add(new NodeDisplacement<string, string, string>(objects[j], loadcaseNames[j], step, fx[j], fy[j], fz[j], mx[j], my[j], mz[j]));

                        NodeDisplacement nd = new NodeDisplacement();
                        nd.Case = loadcaseIds[j];
                        nd.ObjectId = nodeIds[i];
                        nd.RX = mx[j];
                        nd.RY = my[j];
                        nd.RZ = mz[j];
                        nd.UX = fx[j];
                        nd.UY = fy[j];
                        nd.UZ = fz[j];
                        nd.TimeStep = stepNum[j];
                        nodeDisplacements.Add(nd);
                    }
                }
            }

            return nodeDisplacements;

        }

        public static List<NodeReaction> GetNodeReaction(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {
            List<string> loadcaseIds = new List<string>();
            List<string> nodeIds = new List<string>();
            List<NodeReaction> nodeReactions = new List<NodeReaction>();

            if (ids == null)
            {
                int nodes = 0;
                string[] names = null;
                model.PointObj.GetNameList(ref nodes, ref names);
                nodeIds = names.ToList();
            }

            if (cases == null)
            {
                int casesCount = 0;
                string[] names = null;
                model.LoadCases.GetNameList(ref casesCount, ref names);
                loadcaseIds = names.ToList();
                model.RespCombo.GetNameList(ref casesCount, ref names);
                loadcaseIds.AddRange(names);
            }

            int resultCount = 0;
            string[] loadcaseNames = null;
            string[] objects = null;
            string[] elm = null;
            string[] stepType = null;
            double[] stepNum = null;

            double[] fx = null;
            double[] fy = null;
            double[] fz = null;
            double[] mx = null;
            double[] my = null;
            double[] mz = null;


            model.Results.Setup.DeselectAllCasesAndCombosForOutput();

            for (int loadcase = 0; loadcase < loadcaseIds.Count; loadcase++)
            {
                if (model.Results.Setup.SetCaseSelectedForOutput(loadcaseIds[loadcase]) != 0)
                {
                    model.Results.Setup.SetComboSelectedForOutput(loadcaseIds[loadcase]);
                }
            }


            //List<NodeReaction<string, string, string>> nodeForces = new List<NodeReaction<string, string, string>>();
            for (int i = 0; i < nodeIds.Count; i++)
            {
                int ret = model.Results.JointReact(nodeIds[i], eItemTypeElm.ObjectElm, ref resultCount, ref objects, ref elm,
                ref loadcaseNames, ref stepType, ref stepNum, ref fx, ref fy, ref fz, ref mx, ref my, ref mz);
                if (ret == 0)
                {
                    for (int j = 0; j < resultCount; j++)
                    {
                        //string step = stepType[j] != null ? stepType[j] == "Max" ? " Max" : stepType[j] == "Min" ? " Min" : "1" : "0";
                        //nodeForces.Add(new NodeReaction<string, string, string>(objects[j], loadcaseNames[j], step, fx[j], fy[j], fz[j], mx[j], my[j], mz[j]));
                        NodeReaction nr = new NodeReaction();
                        nr.Case = loadcaseIds[j];
                        nr.ObjectId = nodeIds[i];
                        nr.MX = mx[j];
                        nr.MY = my[j];
                        nr.MZ = mz[j];
                        nr.FX = fx[j];
                        nr.FY = fy[j];
                        nr.FZ = fz[j];
                        nr.TimeStep = stepNum[j];
                        nodeReactions.Add(nr);
                    }
                }
            }

            return nodeReactions;
        }

        public static List<IResult> GetNodeVelocity(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {
            throw new NotImplementedException("Node Acceleration results is not supported yet!");

        }
        #endregion

        #region bar Results

        public static List<IResult> GetBarDeformation(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            return new List<IResult>();
        }

        public static List<IResult> GetBarForce(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            return new List<IResult>();
        }

        public static List<IResult> GetBarStrain(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            return new List<IResult>();
        }

        public static List<IResult> GetBarStress(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            return new List<IResult>();
        }


        #endregion

        #region Panel Results

        public static List<IResult> GetPanelForce(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            return new List<IResult>();
        }

        public static List<IResult> GetPanelStress(cSapModel model, IList ids = null, IList cases = null, int divisions = 5)
        {

            return new List<IResult>();
        }


        #endregion
    }
}