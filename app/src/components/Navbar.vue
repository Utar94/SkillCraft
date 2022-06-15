<template>
  <div>
    <b-navbar toggleable="lg" type="dark" variant="dark">
      <b-navbar-brand :to="{ name: 'Home' }">
        <img src="@/assets/logo.png" alt="SkillCraft Logo" height="32" />
        SkillCraft
        <b-badge v-if="environment" variant="warning">{{ environment }}</b-badge>
      </b-navbar-brand>

      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav v-if="token">
          <b-nav-item :to="{ name: 'WorldList' }">
            <font-awesome-icon icon="globe" />
            {{ $t('worlds.title') }}
          </b-nav-item>
          <template v-if="world">
            <!-- TODO(fpion): menu levels? -->
            <b-nav-item :to="{ name: 'AspectList' }">
              <!-- TODO(fpion): icon? -->
              <font-awesome-icon icon="cog" />
              {{ $t('aspects.title') }}
            </b-nav-item>
            <b-nav-item :to="{ name: 'CustomizationList' }">
              <!-- TODO(fpion): icon? -->
              <font-awesome-icon icon="cog" />
              {{ $t('customizations.title') }}
            </b-nav-item>
            <b-nav-item :to="{ name: 'NatureList' }">
              <!-- TODO(fpion): icon? -->
              <font-awesome-icon icon="cog" />
              {{ $t('natures.title') }}
            </b-nav-item>
            <b-nav-item :to="{ name: 'LanguageList' }">
              <!-- TODO(fpion): icon? -->
              <font-awesome-icon icon="cog" />
              {{ $t('languages.title') }}
            </b-nav-item>
          </template>
        </b-navbar-nav>

        <b-navbar-nav class="ml-auto">
          <!-- <b-nav-form>
            <b-input-group>
              <b-form-input size="sm" :placeholder="$t('actions.search')" />
              <b-input-group-append>
                <icon-button class="my-2 my-sm-0" icon="search" size="sm" />
              </b-input-group-append>
            </b-input-group>
          </b-nav-form> -->

          <!-- TODO(fpion): French -->
          <!-- <b-nav-item-dropdown v-if="otherLocales.length" :text="localeName" right>
            <b-dropdown-item v-for="locale in otherLocales" :key="locale.value" :active="locale.value === $i18n.locale" @click="translate(locale.value)">
              {{ locale.text }}
            </b-dropdown-item>
          </b-nav-item-dropdown> -->

          <template v-if="token">
            <b-nav-text v-if="world">{{ world.name }}</b-nav-text>
            <b-nav-item-dropdown right>
              <template #button-content>
                <img v-if="picture" :src="picture" alt="Avatar" class="rounded-circle" width="24" height="24" />
                <v-gravatar v-else class="rounded-circle" :email="email" :size="24" />
              </template>
              <b-dropdown-item :to="{ name: 'Profile' }">
                <font-awesome-icon icon="user" />
                {{ name }}
              </b-dropdown-item>
              <b-dropdown-item @click="doSignOut">
                <font-awesome-icon icon="sign-out-alt" />
                {{ $t('user.signOut') }}
              </b-dropdown-item>
            </b-nav-item-dropdown>
          </template>
          <template v-else>
            <b-nav-item :to="{ name: 'SignIn' }">
              <font-awesome-icon icon="sign-in-alt" />
              {{ $t('signIn.title') }}
            </b-nav-item>
            <b-nav-item :to="{ name: 'SignUp' }">
              <font-awesome-icon icon="user" />
              {{ $t('signUp.title') }}
            </b-nav-item>
          </template>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
  </div>
</template>

<script>
import jwt from 'jsonwebtoken'
import locales from '@/i18n/locales.json'
import { localize } from 'vee-validate'
import { mapActions, mapState } from 'vuex'
import { signOut } from '@/api/identity'

export default {
  data: () => ({
    loading: false
  }),
  computed: {
    ...mapState(['locale', 'token', 'world']),
    email() {
      return this.token ? jwt.decode(this.token.access_token).email : null
    },
    environment() {
      return process.env.VUE_APP_ENV
    },
    localeName() {
      return locales[this.$i18n.locale]
    },
    name() {
      return (this.token ? jwt.decode(this.token.access_token).name : null) ?? this.$i18n.t('user.title')
    },
    otherLocales() {
      return this.$i18n.availableLocales
        .map(value => ({
          text: locales[value],
          value
        }))
        .filter(({ text }) => typeof text === 'string' && text.length > 0 && text !== this.localeName)
        .sort((a, b) => (a.text > b.text ? 1 : a.text < b.text ? -1 : 0))
    },
    picture() {
      return this.token ? jwt.decode(this.token.access_token).picture : null
    }
  },
  methods: {
    ...mapActions(['translate']),
    async doSignOut() {
      if (!this.loading) {
        this.loading = true
        try {
          const { refresh_token } = this.token
          await signOut({ refresh_token })
          this.$store.dispatch('signOut')
          localStorage.removeItem(process.env.VUE_APP_TOKEN_STORAGE_KEY)
          this.$router.push({ name: 'SignIn' })
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    locale: {
      immediate: true,
      handler(locale) {
        if (locale) {
          this.$i18n.locale = locale
          localize(locale)
        }
      }
    }
  }
}
</script>
