---
name: TournéeVéto Fieldwork System
colors:
  surface: '#f1fcf4'
  surface-dim: '#d1ddd5'
  surface-bright: '#f1fcf4'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#ebf6ee'
  surface-container: '#e5f1e8'
  surface-container-high: '#dfebe3'
  surface-container-highest: '#dae5dd'
  on-surface: '#141e19'
  on-surface-variant: '#414844'
  inverse-surface: '#28332d'
  inverse-on-surface: '#e8f4eb'
  outline: '#717974'
  outline-variant: '#c1c8c2'
  surface-tint: '#3f6654'
  primary: '#00281a'
  on-primary: '#ffffff'
  primary-container: '#163e2e'
  on-primary-container: '#80a994'
  inverse-primary: '#a5d0ba'
  secondary: '#3c6377'
  on-secondary: '#ffffff'
  secondary-container: '#bde5fd'
  on-secondary-container: '#41677c'
  tertiary: '#3b1800'
  on-tertiary: '#ffffff'
  tertiary-container: '#5b2900'
  on-tertiary-container: '#f2822e'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#c1ecd5'
  primary-fixed-dim: '#a5d0ba'
  on-primary-fixed: '#002115'
  on-primary-fixed-variant: '#274e3d'
  secondary-fixed: '#c1e8ff'
  secondary-fixed-dim: '#a4cce3'
  on-secondary-fixed: '#001e2b'
  on-secondary-fixed-variant: '#234b5e'
  tertiary-fixed: '#ffdbc7'
  tertiary-fixed-dim: '#ffb689'
  on-tertiary-fixed: '#311300'
  on-tertiary-fixed-variant: '#733500'
  background: '#f1fcf4'
  on-background: '#141e19'
  surface-variant: '#dae5dd'
typography:
  headline-xl:
    fontFamily: Inter
    fontSize: 36px
    fontWeight: '800'
    lineHeight: 44px
    letterSpacing: -0.02em
  headline-xl-mobile:
    fontFamily: Inter
    fontSize: 28px
    fontWeight: '800'
    lineHeight: 36px
    letterSpacing: -0.01em
  headline-lg:
    fontFamily: Inter
    fontSize: 26px
    fontWeight: '700'
    lineHeight: 34px
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Inter
    fontSize: 22px
    fontWeight: '700'
    lineHeight: 30px
  headline-md:
    fontFamily: Inter
    fontSize: 20px
    fontWeight: '700'
    lineHeight: 28px
  ear-tag-display:
    fontFamily: Inter
    fontSize: 32px
    fontWeight: '900'
    lineHeight: 36px
    letterSpacing: 0.04em
  body-lg:
    fontFamily: Inter
    fontSize: 18px
    fontWeight: '500'
    lineHeight: 26px
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '500'
    lineHeight: 24px
  body-sm:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
  label-tactile:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '700'
    lineHeight: 20px
    letterSpacing: 0.02em
  label-badge:
    fontFamily: Inter
    fontSize: 13px
    fontWeight: '700'
    lineHeight: 16px
    letterSpacing: 0.04em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  gutter: 1rem
  gutter-desktop: 1.5rem
  margin: 1rem
  margin-tablet: 1.5rem
  margin-desktop: 2rem
  space-xs: 0.25rem
  space-sm: 0.5rem
  space-md: 0.75rem
  space-lg: 1.25rem
  space-xl: 2rem
---

## Brand & Style

This design system is purpose-built for farm animal practitioners conducting ambulatory herd health rounds in Quebec dairy barns. The interface balances high-efficiency clinical utility with extreme physical environmental constraints: harsh lighting reflections off concrete, muddy or wet tablet screens, gloved touch manipulation, and offline operation inside insulated metal barn structures.

The aesthetic fuses **Utilitarian Functionalism** with **Tactile Field Tooling**. Rather than mimicking fragile, low-contrast consumer mobile software, this design system acts like ruggedized veterinary instrumentation: high structural contrast, immediate visual affordances, high-definition state persistence, and deliberate touch targets that tolerate gloved or wet inputs. The interface must inspire operational confidence, immediate orientation from standing height (at 1 meter reading distance), and calm clarity under time-sensitive, physically demanding conditions.

## Colors

The palette is calibrated specifically for harsh, non-controlled barn lighting (incandescent fixtures, high-pressure sodium, direct overhead sunlight through skylights, or extreme winter glare). The interface defaults strictly to light mode to avoid high-reflection mirror effects on glass screens in bright barns.

- **Primary (`#163E2E`)**: Deep Forest Green. Used for primary navigation bars, dominant action triggers, validated states, and core branding. Exceeds AAA contrast ratios on the off-white background.
- **Secondary (`#1F485B`)**: Petrol Blue. Used for secondary metadata chips, herd inventory tallies, gestation markers, and secondary action tiers.
- **Tertiary (`#C25E00`)**: Warm Amber. Reserved strictly for warnings, withdrawal periods (délais d'attente), high somatic cell counts (CCS critique), and sync alerts. **Rule**: Amber is never deployed as an isolated indicator; it must always be accompanied by an iconography glyph and literal French status text.
- **Neutral (`#18221D`)**: Charcoal Moss. Used for primary typography and high-definition structural borders (1.5px–2px). Ensures crisp legibility without the eye-fatiguing glare of raw `#000000`.
- **Canvas / Surfaces**:
  - `surface-canvas`: `#F4F6F0` (Anti-reflective, warm matte bone background).
  - `surface-card`: `#FFFFFF` (Pristine, high-contrast readable container).
  - `surface-subtle`: `#E7EBE2` (Grounded inset rows, structural divider lines, table headers).
  - `support-sage`: `#4E775D` (Completed herd tasks, neutral confirmations).

## Typography

The design system uses **Inter** across all typographical roles to leverage its high x-height, wide apertures, open counters, and legible numeric figures.

To accommodate veterinarians standing upright while inspecting stanchion stalls or freestall alleys with a tablet mounted on a forearm strap or held in one hand:
- Numerical values—especially ear tags (numéros de boucle ATQ / RFID)—use the dedicated `ear-tag-display` token, featuring a heavy font weight (`900`) and slight tabular tracking to eliminate ambiguity between numerals `8`, `0`, `3`, and `6`.
- Body text is set intentionally one step larger than desktop standard (`16px–18px`) and no body copy ever drops below 500 medium weight.
- Small subtexts and labels are kept at a minimum of `13px` with uppercase or bold treatments, preventing vibration or screen smudges from obliterating clinical instructions.

## Layout & Spacing

Layout geometry follows a **Fluid Touch Column System** with hard minimum spatial tolerances. In dairy barn practice, accidental adjacent triggers from gloved thumb swipes or water droplets must be avoided.

- **Grid Architecture**: Single column fluid width for mobile handhelds (<768px), dual-column master/detail layout for rugged tablets (768px–1199px), and a locked 3-column triage grid for desktop vet-station clinics (≥1200px).
- **Touch Perimeter**: Every clickable container, row selector, or checkbox strictly enforces an isolation boundary of at least `space-sm` (8px) between touch envelopes, with physical dimensions never falling under 56px in height or width.
- **Fixed Top & Bottom Command Shelves**: Critical actions (e.g., Quick Add Cow, Scan RFID, Offline Cache Status) are pinned to anchored persistent bars on mobile/tablet viewports to keep reach zones immediate without requiring vertical scrolling inside long stall lists.

## Elevation & Depth

Because subtle drop shadows wash out completely under direct barn illumination and overhead fluorescent tubes, this design system avoids soft multi-layer shadow blurs. Depth and hierarchy are achieved entirely through **Structural Borders & Tonal Layering**:

- **Low-Altitude Grounding**: Cards, inputs, and interactive surfaces utilize a solid `1.5px` border (or `2px` for focused/active states) in `#18221D` or `#CCD4C8`, sharply setting objects apart from the matte `#F4F6F0` background.
- **Physical Press Depth**: Instead of soft elevation shadows, buttons and actionable cards use a subtle flat inset or a hard 2px offset border (`box-shadow: 0px 2px 0px #18221D`) to indicate tactile depresseability. When pressed, the offset translates 2px down, delivering instant physical confirmation even when audio feedback is drowned out by barn ventilation machinery.
- **Modals and Drawers**: Presented with an opaque 60% neutral-scrim backdrop (`rgba(24, 34, 29, 0.65)`) with pure white card surfaces bordered by a prominent `2px` neutral rim.

## Shapes

The design system standardizes on a bounded roundedness index of `2` (8px to 12px for primary corners):
- **Base Components**: Inputs, buttons, and state chips use `8px` (`0.5rem`) to maintain structural precision and industrial robustness.
- **Cards and Dialog Containers**: Container cards and bottom sheets use `12px` (`0.75rem`), which softens the perimeter while remaining distinct from the soft, rounded aesthetic typical of lifestyle applications.
- **No Full-Pills for Core Actions**: Circular pill buttons are strictly avoided for clinical actions (vêlage, insémination, etc.), as rectangular profiles maximize available tap areas and accommodate long dual-line French terminology without text truncation.

## Components

### Buttons & Gloved Triggers
- **Dimensions**: Universal minimum touch footprint of `56px × 56px`.
- **Primary**: Background `#163E2E`, text `#FFFFFF`, border `2px solid #163E2E`. Active state darkens to `#0E291F` with immediate `scale(0.98)` tactile feedback.
- **Secondary**: Background `#FFFFFF`, text `#163E2E`, border `2px solid #163E2E`.
- **Warning / Alert**: Background `#C25E00`, text `#FFFFFF`, border `2px solid #944700`.

### Status Badges & Offline Markers
- State badges never use color alone. Every badge incorporates an SVG icon (18px) alongside uppercase or bold descriptive text.
- **Offline Storage Badge**: Pinned to the top right of the navigation header. Features an offline disk icon + label `« Hors ligne (stocké sur l'appareil) »` on an opaque neutral tint background (`#E7EBE2`, text `#18221D`, border `1.5px solid #18221D`).
- **Clinical Task Status**:
  - `À faire`: Secondary petrol border (`#1F485B`), white background, clock icon.
  - `Fait`: Solid sage green (`#4E775D`), white text, checkmark icon.

### Animal Identification Cards
- High-durability cards formatted for rapid reading down an alley:
  - Prominent ear-tag block `#3891` rendered in `ear-tag-display` (`32px` bold Inter) against a dedicated high-contrast tag badge.
  - Secondary metadata clearly segmented: Days in Milk (Jours de lait), Parity (Lactation), and Reproduction status.
  - **Quick Action Bar (Régie laitière)**: Embedded 56px action buttons directly inside the card row:
    - `[Vêlage]`
    - `[Tarissement]`
    - `[Insémination]`
    - `[DG]` (Diagnostic de gestation)
    - `[CCS]` (Contrôle cellules somatiques)
  - Selecting an action expands an in-place entry matrix without navigating away from the herd queue.

### Checkboxes, Radios, and Quantitative Toggles
- Minimum check target `32px × 32px` visual box centered inside a `56px × 56px` tap area.
- Unchecked: `2px solid #18221D`, background `#FFFFFF`.
- Checked: `#163E2E` filled with a bold `3px` white checkmark icon.

### Input Fields & Steppers
- All numeric inputs (e.g., dosage mL, body condition score 1-5, somatic cell count) provide large oversized stepper buttons (`-` and `+` at 56px) flanking the input field, enabling instant modification without requiring the software keyboard to obscure the animal list.
- Borders: Crisp `2px solid #18221D`. Focus state changes to `#163E2E` with a `2px` outer highlight.