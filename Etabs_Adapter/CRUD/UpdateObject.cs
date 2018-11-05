﻿using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties;
using BH.oM.Structure.Loads;
using BH.Engine.Structure;
using BH.Engine.Geometry;
using BH.oM.Common.Materials;
using BH.Engine.ETABS;

namespace BH.Adapter.ETABS
{
    public partial class ETABSAdapter
    {
        /***************************************************/
        /**** Adapter override methods                  ****/
        /***************************************************/

        protected override bool UpdateObjects<T>(IEnumerable<T> objects)
        {
            if (typeof(T) == typeof(Node))
            {
                return UpdateObjects(objects as IEnumerable<Node>);
            }
            else
                return base.UpdateObjects<T>(objects);
        }

        /***************************************************/

        private bool UpdateObjects(IEnumerable<Node> nodes)
        {
            bool sucess = true;
            foreach (Node bhNode in nodes)
            {
                if (bhNode.Constraint != null)
                {
                    string name = bhNode.CustomData[AdapterId].ToString();

                    bool[] restraint = new bool[6];
                    restraint[0] = bhNode.Constraint.TranslationX == DOFType.Fixed;
                    restraint[1] = bhNode.Constraint.TranslationY == DOFType.Fixed;
                    restraint[2] = bhNode.Constraint.TranslationZ == DOFType.Fixed;
                    restraint[3] = bhNode.Constraint.RotationX == DOFType.Fixed;
                    restraint[4] = bhNode.Constraint.RotationY == DOFType.Fixed;
                    restraint[5] = bhNode.Constraint.RotationZ == DOFType.Fixed;

                    double[] spring = new double[6];
                    spring[0] = bhNode.Constraint.TranslationalStiffnessX;
                    spring[1] = bhNode.Constraint.TranslationalStiffnessY;
                    spring[2] = bhNode.Constraint.TranslationalStiffnessZ;
                    spring[3] = bhNode.Constraint.RotationalStiffnessX;
                    spring[4] = bhNode.Constraint.RotationalStiffnessY;
                    spring[5] = bhNode.Constraint.RotationalStiffnessZ;

                    sucess &= m_model.PointObj.SetRestraint(name, ref restraint) == 0;
                    sucess &= m_model.PointObj.SetSpring(name, ref spring) == 0;
                }
            }

            return sucess;
        }

        /***************************************************/
    }
}