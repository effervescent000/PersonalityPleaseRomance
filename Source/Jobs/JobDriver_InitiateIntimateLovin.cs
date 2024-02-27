using Personality.Core;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Personality.Romance;

public class JobDriver_InitiateIntimateLovin : JobDriver
{
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

        Toil goToTarget = Toils_Interpersonal.GotoInteractablePosition(TargetPawnIndex);
        goToTarget.socialMode = RandomSocialMode.Off;
        goToTarget.AddFailCondition(() => !CoreGeneralHelper.IsTargetInRange(Actor, TargetPawn));
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
                Actor.ThrowHeart();
            }
        };
        proposeCasualLovin.AddFailCondition(() => !TargetPawn.IsOk());
        yield return proposeCasualLovin;

        Toil awaitResponse = new()
        {
            defaultCompleteMode = ToilCompleteMode.Instant,
            initAction = delegate
            {
                targetAccepted = RomanceHelper.DoesTargetAcceptHookup(Actor, TargetPawn);
            }
        };
        awaitResponse.AddFailCondition(() => !DidTargetAccept);
        yield return awaitResponse;

        Toil giveLovinJobsOrEnd = new()
        {
            defaultCompleteMode = ToilCompleteMode.Instant,
            initAction = delegate
            {
                if (!DidTargetAccept)
                {
                    FleckMaker.ThrowMetaIcon(TargetPawn.Position, TargetPawn.Map, FleckDefOf.IncapIcon);
                    RomanceComp comp = pawn.GetComp<RomanceComp>();
                    comp.RomanceTracker.RejectionList.Add(new RejectionItem(TargetPawn));
                    Actor.needs.mood.thoughts.memories.TryGainMemory(RomanceThoughtDefOf.PPR_TurnedMeDownForHookup, TargetPawn);
                    TargetPawn.needs.mood.thoughts.memories.TryGainMemory(RomanceThoughtDefOf.PPR_HadToRejectSomeoneForHookup, Actor);
                }
                else
                {
                    TargetPawn.ThrowHeart();
                    Actor.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(RomanceJobDefOf.PP_DoIntimateLovin, TargetPawn, Bed, Bed.GetSleepingSlotPos(0)), JobTag.SatisfyingNeeds);
                    TargetPawn.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(RomanceJobDefOf.PP_DoIntimateLovin, Actor, Bed, Bed.GetSleepingSlotPos(1)), JobTag.SatisfyingNeeds);
                    TargetPawn.jobs.EndCurrentJob(JobCondition.InterruptOptional);
                    Actor.jobs.EndCurrentJob(JobCondition.InterruptOptional);
                }
            }
        };

        yield return giveLovinJobsOrEnd;
    }
}