import {
    animate,
    state,
    style,
    transition,
    trigger,
} from '@angular/animations';

export const onSideNavChange = trigger('onSideNavChange', [
    transition('close => open', animate('250ms ease-in')),
    transition('open => close', animate('250ms ease-in')),
]);

export const onMainContentChange = trigger('onMainContentChange', [
    state(
        'hidden',
        style({
            'margin-left': '100px',
        })
    ),
    state(
        'close',
        style({
            'margin-left': '150px',
        })
    ),
    state(
        'open',
        style({
            'margin-left': '200px',
        })
    ),
    transition('* => hidden', animate('250ms ease-in')),
    transition('* => close', animate('250ms ease-in')),
    transition('* => open', animate('250ms ease-in')),
    transition('hidden => *', animate('250ms ease-in')),
    transition('close => *', animate('250ms ease-in')),
    transition('open => *', animate('250ms ease-in')),
]);

export const animateText = trigger('animateText', [
    state(
        'hide',
        style({
            display: 'none',
            opacity: 0,
        })
    ),
    state(
        'show',
        style({
            display: 'block',
            opacity: 1,
        })
    ),
    transition('close => open', animate('350ms ease-in')),
    transition('open => close', animate('200ms ease-out')),
]);
