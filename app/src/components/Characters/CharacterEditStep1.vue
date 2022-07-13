<template>
  <b-container>
    <h1 v-t="'character.title.step1'" />
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <h2 v-t="'character.attributes.bases'" />
        <b-row>
          <form-field
            class="col"
            id="agility"
            label="attribute.options.Agility"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.agility"
          />
          <form-field
            class="col"
            id="coordination"
            label="attribute.options.Coordination"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.coordination"
          />
          <form-field
            class="col"
            id="intellect"
            label="attribute.options.Intellect"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.intellect"
          />
          <form-field
            class="col"
            id="mind"
            label="attribute.options.Mind"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.mind"
          />
          <form-field
            class="col"
            id="presence"
            label="attribute.options.Presence"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.presence"
          />
          <form-field
            class="col"
            id="sensitivity"
            label="attribute.options.Sensitivity"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.sensitivity"
          />
          <form-field
            class="col"
            id="vigor"
            label="attribute.options.Vigor"
            :minValue="3"
            :maxValue="11"
            required
            type="number"
            v-model.number="attributeBases.vigor"
          />
        </b-row>
        <p :class="{ 'text-success': remainingPoints === 0, 'text-danger': remainingPoints !== 0 }">
          {{ $t('character.remainingPoints', { value: remainingPoints }) }}
        </p>
        <h2 v-t="'character.aspects'" />
        <b-row>
          <aspect-select
            :aspects="aspects"
            class="col"
            :exclude="aspect2 ? [aspect2.id] : []"
            id="aspect1"
            label="character.aspect1"
            required
            v-model="aspect1"
          />
          <aspect-select
            :aspects="aspects"
            class="col"
            :exclude="aspect1 ? [aspect1.id] : []"
            id="aspect2"
            label="character.aspect2"
            required
            v-model="aspect2"
          />
        </b-row>
        <template v-if="aspect1 && aspect2">
          <h4 v-t="'character.attributes.mandatory.title'" />
          <b-row>
            <form-select
              class="col"
              id="bestAttribute"
              label="character.attributes.best"
              :options="mandatoryAttributes.filter(({ value }) => value !== worstAttribute)"
              placeholder="character.attributes.placeholder"
              required
              v-model="bestAttribute"
            />
            <form-select
              class="col"
              id="worstAttribute"
              label="character.attributes.worst"
              :options="mandatoryAttributes.filter(({ value }) => value !== bestAttribute)"
              placeholder="character.attributes.placeholder"
              required
              v-model="worstAttribute"
            />
          </b-row>
          <p>{{ $t('character.attributes.mandatory.other', { attributes: otherMandatoryAttributes.join(', ') }) }}</p>
          <h4 v-t="'character.attributes.optional.title'" />
          <b-row>
            <form-select
              class="col"
              id="optionalAttribute1"
              label="character.attributes.optional.label1"
              :options="optionalAttributes.filter(({ value }) => value !== optionalAttribute2)"
              placeholder="character.attributes.placeholder"
              required
              v-model="optionalAttribute1"
            />
            <form-select
              class="col"
              id="optionalAttribute2"
              label="character.attributes.optional.label2"
              :options="optionalAttributes.filter(({ value }) => value !== optionalAttribute1)"
              placeholder="character.attributes.placeholder"
              required
              v-model="optionalAttribute2"
            />
          </b-row>
        </template>
        <div class="my-2">
          <icon-submit class="mx-1" :disabled="remainingPoints !== 0 || !hasChanges" icon="arrow-right" text="actions.next" variant="primary" />
          <icon-button class="mx-1" icon="ban" text="actions.cancel" @click="cancel" />
          <!-- TODO(fpion): confirm cancel -->
        </div>
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import AspectSelect from './AspectSelect.vue'
import { getAspects } from '@/api/aspects'
import { mapActions, mapState } from 'vuex'

export default {
  components: {
    AspectSelect
  },
  data: () => ({
    aspect1: null,
    aspect2: null,
    aspects: [],
    attributeBases: {
      agility: 7,
      coordination: 7,
      intellect: 7,
      mind: 7,
      presence: 7,
      sensitivity: 7,
      vigor: 7
    },
    bestAttribute: null,
    loaded: false,
    optionalAttribute1: null,
    optionalAttribute2: null,
    worstAttribute: null
  }),
  computed: {
    ...mapState(['characterCreation']),
    hasChanges() {
      return Object.values(this.attributeBases).some(value => value !== 7) || this.aspect1 !== null || this.aspect2 !== null
    },
    mandatoryAttributes() {
      const attributes = []
      if (this.aspect1) {
        attributes.push(this.aspect1.mandatoryAttribute1)
        attributes.push(this.aspect1.mandatoryAttribute2)
      }
      if (this.aspect2) {
        attributes.push(this.aspect2.mandatoryAttribute1)
        attributes.push(this.aspect2.mandatoryAttribute2)
      }
      return this.orderBy(
        Array.from(new Set(attributes)).map(attribute => ({
          text: this.$i18n.t(`attribute.options.${attribute}`),
          value: attribute
        })),
        'text'
      )
    },
    optionalAttributes() {
      const attributes = []
      if (this.aspect1) {
        attributes.push({ text: this.$i18n.t(`attribute.options.${this.aspect1.optionalAttribute1}`), value: `1_${this.aspect1.optionalAttribute1}` })
        attributes.push({ text: this.$i18n.t(`attribute.options.${this.aspect1.optionalAttribute2}`), value: `1_${this.aspect1.optionalAttribute2}` })
      }
      if (this.aspect2) {
        attributes.push({ text: this.$i18n.t(`attribute.options.${this.aspect2.optionalAttribute1}`), value: `2_${this.aspect2.optionalAttribute1}` })
        attributes.push({ text: this.$i18n.t(`attribute.options.${this.aspect2.optionalAttribute2}`), value: `2_${this.aspect2.optionalAttribute2}` })
      }
      return this.orderBy(attributes, 'text')
    },
    otherMandatoryAttributes() {
      const attributes = []
      if (this.aspect1) {
        attributes.push(this.aspect1.mandatoryAttribute1)
        attributes.push(this.aspect1.mandatoryAttribute2)
      }
      if (this.aspect2) {
        attributes.push(this.aspect2.mandatoryAttribute1)
        attributes.push(this.aspect2.mandatoryAttribute2)
      }
      const bestIndex = attributes.findIndex(attribute => attribute === this.bestAttribute)
      if (bestIndex >= 0) {
        attributes.splice(bestIndex, 1)
      }
      const worstIndex = attributes.findIndex(attribute => attribute === this.worstAttribute)
      if (worstIndex >= 0) {
        attributes.splice(worstIndex, 1)
      }
      return this.orderBy(attributes)
    },
    payload() {
      return {
        aspect1Id: this.aspect1?.id ?? null,
        aspect2Id: this.aspect2?.id ?? null,
        creation: {
          attributeBases: { ...this.attributeBases },
          bestAttribute: this.bestAttribute,
          worstAttribute: this.worstAttribute,
          mandatoryAttribute1: this.otherMandatoryAttributes[0] ?? null,
          mandatoryAttribute2: this.otherMandatoryAttributes[1] ?? null,
          optionalAttribute1: this.optionalAttribute1,
          optionalAttribute2: this.optionalAttribute2
        }
      }
    },
    remainingPoints() {
      return 7 * 7 + 8 - this.totalPoints
    },
    totalPoints() {
      return Object.values(this.attributeBases).reduce((a, b) => a + b, 0)
    }
  },
  methods: {
    ...mapActions(['resetCharacterCreation', 'saveCharacterCreationStep1']),
    cancel() {
      this.resetCharacterCreation()
      this.$router.push({ name: 'CharacterList' })
    },
    checkAspectAttributes() {
      if (!this.mandatoryAttributes.find(({ value }) => value === this.bestAttribute)) {
        this.bestAttribute = null
      }
      if (!this.mandatoryAttributes.find(({ value }) => value === this.worstAttribute)) {
        this.worstAttribute = null
      }
      if (!this.optionalAttributes.find(({ value }) => value === this.optionalAttribute1)) {
        this.optionalAttribute1 = null
      }
      if (!this.optionalAttributes.find(({ value }) => value === this.optionalAttribute2)) {
        this.optionalAttribute2 = null
      }
    },
    setModel(model) {
      this.aspect1 = this.aspects.find(({ id }) => id === model.aspect1Id)
      this.aspect2 = this.aspects.find(({ id }) => id === model.aspect2Id)
      this.attributeBases.agility = model.creation.attributeBases.agility
      this.attributeBases.coordination = model.creation.attributeBases.coordination
      this.attributeBases.intellect = model.creation.attributeBases.intellect
      this.attributeBases.mind = model.creation.attributeBases.mind
      this.attributeBases.presence = model.creation.attributeBases.presence
      this.attributeBases.sensitivity = model.creation.attributeBases.sensitivity
      this.attributeBases.vigor = model.creation.attributeBases.vigor
      this.bestAttribute = model.creation.bestAttribute
      this.optionalAttribute1 = model.creation.optionalAttribute1
      this.optionalAttribute2 = model.creation.optionalAttribute2
      this.worstAttribute = model.creation.worstAttribute
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            this.$router.push({ name: 'CharacterEditStep2' })
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
    try {
      const { data } = await getAspects({ deleted: false, sort: 'Name' })
      this.aspects = data.items
      if (this.characterCreation.step1) {
        this.setModel(this.characterCreation.step1)
      }
    } catch (e) {
      this.handleError(e)
    }
    this.loaded = true
  },
  watch: {
    aspect1: {
      deep: true,
      immediate: true,
      handler() {
        this.checkAspectAttributes()
      }
    },
    aspect2: {
      deep: true,
      immediate: true,
      handler() {
        this.checkAspectAttributes()
      }
    },
    payload: {
      deep: true,
      immediate: true,
      handler(payload) {
        if (this.loaded) {
          this.saveCharacterCreationStep1(payload)
        }
      }
    }
  }
}
</script>
