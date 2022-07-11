<template>
  <b-container>
    <h1 v-text="title" />
    <status-detail
      v-if="power"
      :createdAt="new Date(power.createdAt)"
      :deletedAt="power.deletedAt ? new Date(power.deletedAt) : null"
      :updatedAt="power.updatedAt ? new Date(power.updatedAt) : null"
    />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <name-field class="col" v-model="name" />
          <tier-select class="col" :disabled="Boolean(power)" :required="!power" v-model="tier" />
        </b-row>
        <b-row>
          <incantation-select class="col" required :ritual="ritual" v-model="incantation" @ritualInput="ritual = $event" />
          <b-form-group class="col" :label="$t('power.components.label')">
            <b-row>
              <b-form-checkbox class="col" v-model="somatic">{{ $t('power.components.somatic') }}</b-form-checkbox>
              <b-form-checkbox class="col" :checked="material.ingredients && !material.consumed" disabled>{{ $t('power.components.focus') }}</b-form-checkbox>
            </b-row>
            <b-row>
              <b-form-checkbox class="col" v-model="verbal">{{ $t('power.components.verbal') }}</b-form-checkbox>
              <b-form-checkbox class="col" :checked="material.ingredients && material.consumed" disabled>
                {{ $t('power.components.material') }}
              </b-form-checkbox>
            </b-row>
          </b-form-group>
        </b-row>
        <b-row>
          <duration-select class="col" :concentration="concentration" required v-model="duration" @concentrationInput="concentration = $event" />
          <range-field class="col" required v-model="range" />
        </b-row>
        <form-field
          id="ingredients"
          label="power.ingredients.label"
          :maxLength="100"
          placeholder="power.ingredients.placeholder"
          :required="material.consumed"
          v-model="material.ingredients"
        >
          <b-form-checkbox v-model="material.consumed">{{ $t('power.ingredients.consumed') }}</b-form-checkbox>
        </form-field>
        <description-field
          id="globalDescription"
          label="power.descriptions.global.label"
          :maxLength="1000"
          placeholder="power.descriptions.global.placeholder"
          :rows="4"
          v-model="descriptions.global"
        />
        <description-field
          id="firstLevelDescription"
          label="power.descriptions.firstLevel.label"
          :maxLength="1000"
          placeholder="power.descriptions.firstLevel.placeholder"
          required
          :rows="4"
          v-model="descriptions.firstLevel"
        />
        <description-field
          id="secondLevelDescription"
          label="power.descriptions.secondLevel.label"
          :maxLength="1000"
          placeholder="power.descriptions.secondLevel.placeholder"
          required
          :rows="4"
          v-model="descriptions.secondLevel"
        />
        <description-field
          id="thirdLevelDescription"
          label="power.descriptions.thirdLevel.label"
          :maxLength="1000"
          placeholder="power.descriptions.thirdLevel.placeholder"
          required
          :rows="4"
          v-model="descriptions.thirdLevel"
        />
        <div class="my-2">
          <icon-submit v-if="power" class="mx-1" :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
          <icon-submit v-else class="mx-1" :disabled="!hasChanges || loading" icon="plus" :loading="loading" text="actions.create" variant="success" />
          <icon-button class="mx-1" icon="arrow-left" text="actions.back" :to="{ name: 'PowerList' }" />
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import DurationSelect from './DurationSelect.vue'
import IncantationSelect from './IncantationSelect.vue'
import RangeField from './RangeField.vue'
import { createPower, getPower, updatePower } from '@/api/powers'

export default {
  components: {
    DurationSelect,
    IncantationSelect,
    RangeField
  },
  data: () => ({
    concentration: false,
    descriptions: {
      global: null,
      firstLevel: null,
      secondLevel: null,
      thirdLevel: null
    },
    duration: null,
    incantation: null,
    loading: false,
    material: {
      consumed: false,
      ingredients: null
    },
    name: null,
    power: null,
    range: null,
    ritual: false,
    somatic: false,
    tier: null,
    verbal: false
  }),
  computed: {
    hasChanges() {
      return (
        (this.name ?? '') !== (this.power?.name ?? '') ||
        this.tier !== (this.power?.tier ?? null) ||
        this.incantation !== (this.power?.incantation ?? null) ||
        this.ritual !== (this.power?.ritual ?? false) ||
        this.somatic !== (this.power?.somatic ?? false) ||
        this.verbal !== (this.power?.verbal ?? false) ||
        (this.duration === null || isNaN(this.duration) ? null : Number(this.duration)) !== (this.power?.duration ?? null) ||
        this.concentration !== (this.power?.concentration ?? false) ||
        (this.range === null || isNaN(this.range) ? null : Number(this.range)) !== (this.power?.range ?? null) ||
        (this.material.ingredients ?? '') !== (this.power?.ingredients ?? '') ||
        this.material.consumed !== (this.power?.focus ?? false) ||
        (this.descriptions.global ?? '') !== (this.power?.descriptions.global ?? '') ||
        (this.descriptions.firstLevel ?? '') !== (this.power?.descriptions.firstLevel ?? '') ||
        (this.descriptions.secondLevel ?? '') !== (this.power?.descriptions.secondLevel ?? '') ||
        (this.descriptions.thirdLevel ?? '') !== (this.power?.descriptions.thirdLevel ?? '')
      )
    },
    payload() {
      const payload = {
        concentration: this.concentration,
        descriptions: { ...this.descriptions },
        duration: isNaN(this.duration) ? null : Number(this.duration),
        focus: this.material.consumed,
        incantation: this.incantation,
        ingredients: this.material.ingredients,
        name: this.name,
        range: this.range,
        ritual: this.ritual,
        somatic: this.somatic,
        verbal: this.verbal
      }
      if (!this.power) {
        payload.tier = this.tier
      }
      return payload
    },
    title() {
      return this.power?.name ?? this.$i18n.t('power.title')
    }
  },
  methods: {
    setModel(model) {
      this.power = model
      this.concentration = model.concentration
      this.descriptions.global = model.descriptions.global
      this.descriptions.firstLevel = model.descriptions.firstLevel
      this.descriptions.secondLevel = model.descriptions.secondLevel
      this.descriptions.thirdLevel = model.descriptions.thirdLevel
      this.duration = model.duration
      this.incantation = model.incantation
      this.material.consumed = model.focus
      this.material.ingredients = model.ingredients
      this.name = model.name
      this.range = model.range
      this.ritual = model.ritual
      this.somatic = model.somatic
      this.tier = model.tier
      this.verbal = model.verbal
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            if (this.power) {
              const { data } = await updatePower(this.power.id, this.payload)
              this.setModel(data)
              this.toast('success', 'power.updated')
            } else {
              const { data } = await createPower(this.payload)
              this.setModel(data)
              this.toast('success', 'power.created')
            }
            this.$refs.form.reset()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  async created() {
    const id = this.$route.params.id
    if (id && id !== 'new') {
      try {
        const { data } = await getPower(id)
        this.setModel(data)
      } catch (e) {
        if (e.status === 404) {
          return this.$router.push({ name: 'NotFound' })
        }
        this.handleError(e)
      }
    }
  }
}
</script>
