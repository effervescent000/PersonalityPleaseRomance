using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Personality.Romance;

public class JobDriver_InitiateCasualLovin : JobDriver
{
    // im not sure why i have to do this but WBR says to do it
    public bool targetAccepted = true;
    public bool DidTargetAccept => targetAccepted;

    private Pawn Actor => GetActor();
    private Pawn TargetPawn => (Pawn)TargetThingA;
    private Building_Bed Bed => (Building_Bed)TargetThingB;
    private TargetIndex TargetPawnIndex => TargetIndex.A;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        if (TargetPawn == null)
        {
            Log.Warning("TargetPawn is null in JobDriver");
        }
        if (Bed == null)
        {
            Log.Warning("Bed is null in JobDriver");
        }

        this.FailOnDespawnedNullOrForbidden(TargetPawnIndex);
        // TODO add a fail condition for if the target pawn gets too far away

        Toil goToTarget = Toils_Interpersonal.GotoInteractablePosition(TargetPawnIndex);
        goToTarget.socialMode = RandomSocialMode.Off;
        yield return goToTarget;

        Toil wait = Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
        wait.socialMode = RandomSocialMode.Off;
        yield return wait;

        Toil proposeCasualLovin = new()
        {
            defaultCompleteMode = ToilCompleteMode.Delay,
            initAction = delegate
            {
                ticksLeftThisToil = 50;
                FleckMaker.ThrowMetaIcon(Actor.Position, Actor.Map, FleckDefOf.Heart);
            }

        };
        // proposition should fail if target pawn is dead or downed
        yield return proposeCasualLovin;

        Toil awaitResponse = new()
        {
            defaultCompleteMode = ToilCompleteMode.Instant,
            initAction = delegate
            {
                // this should be true or false based on whether target accepts
                targetAccepted = true;
            }
        };
        yield return awaitResponse;

        Toil giveLovinJobsOrEnd = new()
        {
            defaultCompleteMode = ToilCompleteMode.Instant,
            initAction = delegate
            {
                if (!DidTargetAccept)
                {
                    return;
                }
                Actor.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(RomanceJobDefOf.DoCasualLovin, TargetPawn, Bed, Bed.GetSleepingSlotPos(0)), JobTag.SatisfyingNeeds);
                TargetPawn.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(RomanceJobDefOf.DoCasualLovin, Actor, Bed, Bed.GetSleepingSlotPos(1)), JobTag.SatisfyingNeeds);
                Actor.jobs.EndCurrentJob(JobCondition.InterruptOptional);
                TargetPawn.jobs.EndCurrentJob(JobCondition.InterruptOptional);
            }
        };
        
        yield return giveLovinJobsOrEnd;


    }
}
