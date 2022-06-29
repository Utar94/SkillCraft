<template>
  <b-modal :id="id" :title="$t(title)" @hidden="reset">
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <talent-select :exclude="exclude" required :tiers="[0, 1, 2, 3].filter(value => value < tier)" :value="talentId" @talent="selectTalent" />
        <b-form-group>
          <b-form-checkbox v-model="mandatory">{{ $t('class.acquisition.mandatory') }}</b-form-checkbox>
        </b-form-group>
      </b-form>
    </validation-observer>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="cancel()" />
      <icon-button icon="plus" :disabled="!hasChanges" text="actions.add" variant="success" @click="submit(ok)" />
    </template>
  </b-modal>
</template>

<script>
import TalentSelect from '../Talents/TalentSelect.vue'

export default {
  components: {
    TalentSelect
  },
  props: {
    exclude: {
      type: Array,
      default: () => []
    },
    id: {
      type: String,
      default: 'selectTalent'
    },
    tier: {
      type: Number,
      default: 0
    },
    title: {
      type: String,
      default: 'class.acquisition.addTalent'
    }
  },
  data: () => ({
    mandatory: false,
    talent: null,
    talentId: null
  }),
  computed: {
    hasChanges() {
      return this.talent !== null || this.mandatory
    }
  },
  methods: {
    reset() {
      this.talent != null
      this.talentId = null
      this.mandatory = false
    },
    selectTalent(talent) {
      this.talent = talent
      this.talentId = talent.id
    },
    async submit(callback = null) {
      try {
        if (await this.$refs.form.validate()) {
          this.$emit('ok', { callback, mandatory: this.mandatory, talent: this.talent })
          this.$refs.form.reset()
        }
      } catch (e) {
        this.handleError(e)
      }
    }
  }
}
</script>
