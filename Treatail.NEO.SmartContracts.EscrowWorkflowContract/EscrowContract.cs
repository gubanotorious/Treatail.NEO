using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace Treatail.Blockchain.SmartContracts.EscrowContract
{
    public class Workflow
    {
        public BigInteger TotalSteps { get; set; }
        public BigInteger TotalAmount { get; set; }

        public Step[] Steps { get; set; }
    }

    public enum StepStatus
    {
        Pending = 0,
        Completed = 1
    }

    public class Step
    {
        public StepStatus Status { get; set; }
        public BigInteger PaymentAmount { get; set; }
        public string PaymentAddress { get; set; }
    }

    public class EscrowContract : SmartContract
    {
        //private static readonly byte[] _treatailAddress = "AKwhdHvupN2dRrMRTpNAFYgFQZiLmftmz6".ToScriptHash();

        public static object Main(string action, object[] args)
        {
            switch (action)
            {
                case "addworkflow":
                    return AddWorkflowDetails((byte[])args[0], (byte[])args[1]);
                case "addstep":
                    return AddStepDetails((byte[])args[0], (BigInteger)args[1], (byte[])args[2]);
                case "getworkflow":
                    return GetWorkflowDetails((byte[])args[0]);
                case "getstep":
                    return GetStepDetails((byte[])args[0], (BigInteger)args[1]);
                case "getstepstatus":
                    return GetStepStatus((byte[])args[0], (BigInteger)args[1]);
                case "executenextstep":
                    return ExecuteNextStep((byte[])args[0]);
            }

            return false;
        }

        #region Public Functions for Workflow Management

        public static bool ExecuteNextStep(byte[] workflowId)
        {
            //if (!Runtime.CheckWitness(_treatailAddress))
            //{
            //    Runtime.Notify("You do not have permissions to execute workflow steps.");
            //    return false;
            //}

            var workflow = GetWorkflow(workflowId);
            var createdDateString = workflow[0];
            var descriptionString = workflow[1];
            var stepsCountString = workflow[2];
            var totalAmountString = workflow[3];

            var stepsCount = (int)workflow[2];
            for (int i = 0; i < stepsCount; i++)
            {
                var step = GetStep(workflowId, i);
                var stepCompleted = (BigInteger)step[0];
                var stepOwnerAddress = (string)step[1];
                if (stepCompleted == 0)
                {
                    //Send the funds for the current step
                    SendFundsForCompletedStep(workflowId, i);

                    //Mark the current step completed
                    SetStepCompleted(workflowId, i);

                    return true;
                }
            }

            Runtime.Notify("Workflow already completed, no more steps avaialble.");
            return false;
        }

        public static bool AddWorkflowDetails(byte[] workflowId, byte[] workflowDetails)
        {
            //if (!Runtime.CheckWitness(_treatailAddress))
            //{
            //    Runtime.Notify("You do not have permission to create workflows.");
            //    return false;
            //}

            return SetWorkflowDetails(workflowId, workflowDetails);
        }

        public static bool AddStepDetails(byte[] workflowId, BigInteger stepId, byte[] stepDetails)
        {
            //if (!Runtime.CheckWitness(_treatailAddress))
            //{
            //    Runtime.Notify("You do not have permission to add workflow steps.");
            //    return false;
            //}

            return SetStepDetails(workflowId, stepId, stepDetails);
        }

        public static byte[] GetWorkflowDetails(byte[] workflowId)
        {
            return GetWorkflowFromStorage(workflowId);
        }

        public static byte[] GetStepDetails(byte[] workflowId, BigInteger stepId)
        {
            return GetStepDetailsFromStorage(workflowId, stepId);
        }

        public static byte[] GetStepStatus(byte[] workflowId, BigInteger stepId)
        {
            return GetStepStatusFromStorage(workflowId, stepId);
        }

        #endregion

        #region Internal Functions to Retrieve Workflows

        private static object[] GetWorkflow(byte[] workflowId)
        {
            return Deserialize(GetWorkflowFromStorage(workflowId));
        }

        private static object[] GetStep(byte[] workflowId, BigInteger stepId)
        {
            var status = GetStepStatusFromStorage(workflowId, stepId).AsBigInteger();
            var details = Deserialize(GetStepDetailsFromStorage(workflowId, stepId));

            var stepInfo = new object[details.Length + 1];
            stepInfo[0] = status;
            for (int i = 0; i < details.Length; i++)
                stepInfo[i + 1] = details[i];

            return stepInfo;
        }

        #endregion

        #region Storage Helper Functions

        private static byte[] GetWorkflowFromStorage(byte[] workflowId)
        {
            //Make sure we have the params we need
            if (workflowId == null || workflowId.Length == 0)
            {
                Runtime.Log("Insufficient parameters to get workflow details.");
                return null;
            }

            var workflowKey = GetStorageKey("workflow", workflowId, 0);
            return Storage.Get(Storage.CurrentContext, workflowKey);
        }

        private static byte[] GetStepStatusFromStorage(byte[] workflowId, BigInteger stepId)
        {
            //Make sure we have the params we need
            if (workflowId == null || workflowId.Length == 0 || stepId == null || stepId == null)
            {
                Runtime.Log("Insufficient parameters to get step status.");
                return null;
            }

            var stepKey = GetStorageKey("step_status", workflowId, stepId);
            return Storage.Get(Storage.CurrentContext, stepKey);
        }

        private static byte[] GetStepDetailsFromStorage(byte[] workflowId, BigInteger stepId)
        {
            //Make sure we have the params we need
            if (workflowId == null || workflowId.Length == 0 || stepId == null || stepId == null)
            {
                Runtime.Log("Insufficient parameters to get step details.");
                return null;
            }

            var stepKey = GetStorageKey("step", workflowId, stepId);
            return Storage.Get(Storage.CurrentContext, stepKey);
        }

        private static bool SetWorkflowDetails(byte[] workflowId, byte[] details)
        {
            if (workflowId == null || workflowId.Length == 0 || details == null || details.Length == 0)
            {
                Runtime.Log("Insufficient parameters to set workflow details.");
                return false;
            }

            var workflowKey = GetStorageKey("workflow", workflowId, 0);
            Storage.Put(Storage.CurrentContext, workflowKey, details);
            return true;
        }

        private static bool SetStepDetails(byte[] workflowId, BigInteger stepId, byte[] details)
        {
            //Make sure we have the params we need
            if (workflowId == null || workflowId.Length == 0
                || stepId == null || stepId == null
                || details == null || details.Length == 0)
            {
                Runtime.Log("Insufficient parameters to set step details.");
                return false;
            }

            var stepKey = GetStorageKey("step", workflowId, stepId);
            Storage.Put(Storage.CurrentContext, stepKey, details);

            //Default to incomplete
            stepKey = GetStorageKey("step_status", workflowId, stepId);
            Storage.Put(Storage.CurrentContext, stepKey, 0);

            return true;
        }

        private static bool SendFundsForCompletedStep(byte[] workflowId, BigInteger stepId)
        {
            var stepKey = GetStorageKey("step_status", workflowId, stepId);
            var step = Deserialize(Storage.Get(Storage.CurrentContext, stepKey));

            var address = (byte[])step[2];
            //Send funds somehow

            return true;
        }

        private static bool SetStepCompleted(byte[] workflowId, BigInteger stepId)
        {
            var stepKey = GetStorageKey("step_status", workflowId, stepId);
            Storage.Put(Storage.CurrentContext, stepKey, 1);
            return true;
        }

        #endregion

        #region Helpers

        private static string GetStorageKey(string scope, byte[] workflowId, BigInteger stepId)
        {
            //Format is [WorkflowId]
            if (scope == "workflow")
            {
                return workflowId.AsString();
            }
            //Format is [WorkflowId]S[StepId]
            if (scope == "step")
            {
                return string.Concat(workflowId.AsString(), "S", stepId);
            }
            //Format is [WorkflowId]S[StepId]S
            if (scope == "step_status")
            {
                return string.Concat(workflowId.AsString(), "S", stepId, "S");
            }

            return String.Empty;
        }

        private static object[] Deserialize(byte[] bytes)
        {
            string s = bytes.AsString();
            object[] strings = new object[62];
            string str = "";
            int pos = 0;
            foreach (char c in s)
            {
                if (c == '|')
                {
                    strings[pos] = str;
                    pos = pos + 1;
                    str = "";
                }
                else
                {
                    str = string.Concat(str, c.ToString());
                }
            }
            return strings;
        }

        private static byte[] Serialize(object[] strings)
        {
            string str = "";
            for (int i = 0; i < strings.Length; i++)
            {
                str = string.Concat(str, strings[i]);
                if (i == (strings.Length - 1))
                {
                    str = string.Concat(str, "|");
                }
            }
            return str.AsByteArray();
        }

        #endregion
    }
}
