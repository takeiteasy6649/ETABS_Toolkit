/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETABS2016;
using BH.oM.Structure.Loads;
using BH.oM.Structure;
using BH.oM.Structure.Constraints;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Adapter.ETABS
{
    public static partial class Helper
    {

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static LinkConstraint LinkConstraint(string name, eLinkPropType linkType, cSapModel model)
        {

            switch (linkType)
            {
                case eLinkPropType.Linear:
                    return GetLinearLinkConstraint(name, model);
                case eLinkPropType.Damper:
                case eLinkPropType.Gap:
                case eLinkPropType.Hook:
                case eLinkPropType.PlasticWen:
                case eLinkPropType.Isolator1:
                case eLinkPropType.Isolator2:
                case eLinkPropType.MultilinearElastic:
                case eLinkPropType.MultilinearPlastic:
                case eLinkPropType.Isolator3:
                default:
                    Engine.Reflection.Compute.RecordError("Reading of LinkConstraint of type " + linkType + " not implemented");
                    return null;
            }

        }

        /***************************************************/

        private static LinkConstraint GetLinearLinkConstraint(string name, cSapModel model)
        {
            bool[] dof = null;
            bool[] fix = null;
            double[] stiff = null;
            double[] damp = null;
            double dj2 = 0; //Not sure what this is doing
            double dj3 = 0; //Not sure what this is doing
            bool stiffCoupled = false;
            bool dampCoupled = false;
            string notes = null;
            string guid = null;

            model.PropLink.GetLinear(name, ref dof, ref fix, ref stiff, ref damp, ref dj2, ref dj3, ref stiffCoupled, ref dampCoupled, ref notes, ref guid);

            LinkConstraint constraint = new LinkConstraint();

            constraint.Name = name;
            constraint.CustomData[AdapterId] = name;

            constraint.XtoX = fix[0];
            constraint.ZtoZ = fix[1];
            constraint.YtoY = fix[2];
            constraint.XXtoXX = fix[3];
            constraint.YYtoYY = fix[4];
            constraint.ZZtoZZ = fix[5];

            if (stiff != null && stiff.Any(x => x != 0))
                Engine.Reflection.Compute.RecordWarning("No stiffness read for link constraints");

            if (damp != null && damp.Any(x => x != 0))
                Engine.Reflection.Compute.RecordWarning("No damping read for link contraint");

            return constraint;

        }

        /***************************************************/

    }
}
