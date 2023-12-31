<template>
  <validation-provider :name="$t(label).toLowerCase()" :rules="allRules" :vid="id" v-slot="validationContext" slim>
    <b-form-group :label="required ? '' : $t(label)" :label-for="id" :invalid-feedback="validationContext.errors[0]">
      <template #label v-if="required"><span class="text-danger">*</span> {{ $t(label) }}</template>
      <b-input-group>
        <template #prepend>
          <slot name="prepend" />
        </template>
        <b-form-input
          :disabled="disabled"
          :id="id"
          :placeholder="$t(placeholder)"
          :ref="id"
          :state="hasRules ? getValidationState(validationContext) : null"
          :step="step"
          :type="type"
          :value="value"
          @input="$emit('input', $event)"
        />
        <template #append>
          <slot name="append" />
        </template>
      </b-input-group>
      <slot />
    </b-form-group>
  </validation-provider>
</template>

<script>
import { v4 as uuidv4 } from 'uuid'

export default {
  props: {
    disabled: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: () => uuidv4()
    },
    label: {
      type: String,
      default: ''
    },
    maxLength: {
      type: Number
    },
    maxValue: {
      type: Number
    },
    minLength: {
      type: Number
    },
    minValue: {
      type: Number
    },
    placeholder: {
      type: String,
      default: ''
    },
    required: {
      type: Boolean,
      default: false
    },
    rules: {
      type: Object,
      default: null
    },
    step: {
      type: Number
    },
    type: {
      type: String,
      default: 'text'
    },
    value: {}
  },
  computed: {
    allRules() {
      const rules = this.rules ?? {}
      if (typeof this.maxLength === 'number') {
        rules.max = this.maxLength
      }
      if (typeof this.maxValue === 'number') {
        rules.max_value = this.maxValue
      }
      if (typeof this.minLength === 'number') {
        rules.min = this.minLength
      }
      if (typeof this.minValue === 'number') {
        rules.min_value = this.minValue
      }
      if (this.required) {
        rules.required = true
      }
      return rules
    },
    hasRules() {
      return Object.keys(this.allRules).length
    }
  },
  methods: {
    focus() {
      this.$refs[this.id].focus()
    }
  }
}
</script>
