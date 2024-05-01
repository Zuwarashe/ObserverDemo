using System;

namespace ObserverDemo
{
    // Define the Policy class
    public class Policy
    {
        public int PolicyNumber { get; private set; } // Variable: PolicyNumber

        public int NumberOfClaims { get; private set; } // Variable: NumberOfClaims

        // Define an event to notify observers when a claim is completed
        public event EventHandler ClaimCompleted; // Variable: ClaimCompletedEvent

        public Policy(int policyNumber)
        {
            PolicyNumber = policyNumber;
            NumberOfClaims = 0;
        }

        // Method to add a claim and notify observers if it's completed
        public void AddClaim()
        {
            NumberOfClaims++;
            Console.WriteLine($"Policy {PolicyNumber}: Claim added. Total Claims: {NumberOfClaims}");

            // Check if the claim is completed (for demonstration purposes, we'll consider it completed after 3 claims)
            if (NumberOfClaims >= 3)
            {
                OnClaimCompleted(); // Notify observers when the claim limit is reached
            }
        }

        // Method to trigger the ClaimCompleted event
        protected virtual void OnClaimCompleted()
        {
            ClaimCompleted?.Invoke(this, EventArgs.Empty); // Raise the ClaimCompleted event
        }
    }

    // Define the Claim class
    public class Claim
    {
        public int ClaimNumber { get; private set; } // Variable: ClaimNumber

        public bool IsCompleted { get; private set; } // Variable: IsClaimCompleted

        public Claim(int claimNumber)
        {
            ClaimNumber = claimNumber;
            IsCompleted = false;
        }

        // Method to complete the claim and notify the related policy
        public void Complete()
        {
            IsCompleted = true;
            Console.WriteLine($"Claim {ClaimNumber}: Completed");

            // Notify the related policy that the claim is completed
            PolicyManager.NotifyPolicyClaimCompleted(ClaimNumber); // Variable: ClaimNumber
        }
    }

    // Define a static class to manage policies and handle claim completion notification
    public static class PolicyManager
    {
        public static event EventHandler<int> PolicyClaimCompleted; // Variable: PolicyClaimCompletedEvent

        // Method to notify the policy when a claim is completed
        public static void NotifyPolicyClaimCompleted(int claimNumber) // Variable: ClaimNumber
        {
            PolicyClaimCompleted?.Invoke(null, claimNumber); // Raise the PolicyClaimCompleted event
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a policy and claims
            Policy insurancePolicy = new Policy(1); // Variable: insurancePolicy
            Claim carClaim1 = new Claim(101); // Variable: carClaim1
            Claim carClaim2 = new Claim(102); // Variable: carClaim2
            Claim carClaim3 = new Claim(103); // Variable: carClaim3

            // Subscribe to the PolicyClaimCompleted event
            PolicyManager.PolicyClaimCompleted += HandlePolicyClaimCompleted;

            // Add claims to the policy
            insurancePolicy.AddClaim();
            carClaim1.Complete();
            insurancePolicy.AddClaim();
            carClaim2.Complete();
            insurancePolicy.AddClaim();
            carClaim3.Complete();
        }

        // Event handler for PolicyClaimCompleted event
        static void HandlePolicyClaimCompleted(object sender, int claimNumber) // Variables: sender, claimNumber
        {
            Console.WriteLine($"Policy Claim {claimNumber}: All claims completed. Policy closed."); // Variable: claimNumber
        }
    }
}
